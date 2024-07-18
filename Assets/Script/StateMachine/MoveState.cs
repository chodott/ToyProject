using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayableState
{
    private PlayerController _playerController;
    private float accel = 8.0f;

    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;
    }

    public void Update()
    {
        if (_playerController == null) return;



        _playerController.Decelerate();

        _playerController.velocity += _playerController.moveDir.x * accel * Time.deltaTime;
        _playerController.velocity = MathF.Abs(_playerController.velocity) > _playerController.MaxSpeed ?
           _playerController.MaxSpeed * _playerController.moveDir.x : _playerController.velocity;

        _playerController.Move();
    }
}
