using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    private int _playerNum;
    public int PlayerNum { get { return _playerNum; } set { _playerNum = value; } }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {

        }
    }


    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
