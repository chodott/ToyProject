using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkStartHandler : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner _networkRunner;
    public Button MatchButton;
    void Start()
    {

        MatchButton.onClick.AddListener(MatchGame);
    }

    void MatchGame()
    {
        var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }


protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, int scene, Action<NetworkRunner> initalize)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            SessionName = "Test",
            SceneManager = sceneManager,
            PlayerCount = 2,
            MatchmakingMode = MatchmakingMode.RandomMatching
        }) ; 
    }
    
}
