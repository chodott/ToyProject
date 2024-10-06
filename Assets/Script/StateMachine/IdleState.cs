using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayableState
{
    private PlayerController _playerController;
    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;
    }

    public void Update(float deltaTime)
    {
        if (_playerController == null) return;

        _playerController.Decelerate();
        _playerController.Move();
    }
}
