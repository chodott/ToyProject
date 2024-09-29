using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : Singleton<Spawner>, INetworkRunnerCallbacks
{
    public NetworkObject networkSamplePrefab;
    public NetworkObject networkPlayerPrefab;
    [SerializeField]
    private Vector3 _player1SpawnPos = new Vector3(100.0f,0,0);
    [SerializeField]
    private Vector3 _player2SpawnPos = new Vector3(-100.0f, 0, 0);

    private PlayerRef _player1Ref;
    private PlayerRef _player2Ref;

    public int Player1CharacterType;
    public int Player2CharacterType;


    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
    
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsSceneAuthority)
        {
            if (runner.SessionInfo.PlayerCount == 2)
            {
                _player2Ref = player;
                runner.LoadScene(SceneRef.FromIndex(1));
            }
            else
            {
                _player1Ref = player;
            }
        }
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.IsSceneAuthority == false) return;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                {
                    var player1Object = runner.Spawn(networkSamplePrefab, _player1SpawnPos);
                    var player2Object = runner.Spawn(networkSamplePrefab, _player2SpawnPos);
                    runner.SetPlayerObject(_player1Ref, player1Object);
                    runner.SetPlayerObject(_player2Ref, player2Object);

                    SelectUIManager.UIManager.SetDataRpc(player1Object, player2Object);
                    break;

                }

            default:
                {
                    runner.UnloadScene("SelectScene");

                    Player1CharacterType = runner.GetPlayerObject(_player1Ref).GetComponent<SampleCharacter>().FormNum;
                    Player2CharacterType = runner.GetPlayerObject(_player2Ref).GetComponent<SampleCharacter>().FormNum;

                    Destroy(runner.GetPlayerObject(_player1Ref));
                    Destroy(runner.GetPlayerObject(_player2Ref));

                    var player1Object = runner.Spawn(networkPlayerPrefab, _player1SpawnPos);
                    var player2Object = runner.Spawn(networkPlayerPrefab, _player2SpawnPos);
                    runner.SetPlayerObject(_player1Ref, player1Object);
                    runner.SetPlayerObject(_player2Ref, player2Object);

                    BattleUIManager.UIManager.SetDataRpc(player1Object, player2Object, 
                        Player1CharacterType, Player2CharacterType);
                    break;
                }
        }
       

    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {


    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

}
