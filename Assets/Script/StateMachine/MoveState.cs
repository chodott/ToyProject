using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayableState
{
    private PlayerController _playerController;
    private float _accel = 8.0f;

    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;
    }

    public void Update()
    {
        if (_playerController == null) return;



        _playerController.Decelerate();

        _playerController.Velocity += _playerController.MoveDir.x * _accel * Time.deltaTime;
        _playerController.Velocity = MathF.Abs(_playerController.Velocity) > _playerController.MaxSpeed ?
            _playerController.MaxSpeed * _playerController.MoveDir.x : _playerController.Velocity;

        _playerController.Move();
    }
}
