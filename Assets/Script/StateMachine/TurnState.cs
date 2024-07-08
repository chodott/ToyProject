using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState : MonoBehaviour, IPlayableState
{
    private PlayerController _playerController;
    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;

  
    }

    public void Update()
    {
        if (_playerController == null) return;

     
    }
}
