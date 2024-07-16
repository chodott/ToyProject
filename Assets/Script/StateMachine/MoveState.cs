using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayableState
{
    private PlayerController _playerController;
    private float accel = 8.0f;
    public float MaxSpeed { get; set; } = 3.0f;

    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;
    }

    public void Update()
    {
        if (_playerController == null) return;

        _playerController.idle_run_ratio = Mathf.Lerp(_playerController.idle_run_ratio, 1, MaxSpeed * Time.deltaTime);
        Animator animator = _playerController.GetComponent<Animator>();
        animator.SetFloat("idle_run_ratio", _playerController.idle_run_ratio);
        animator.Play("OnGround");

        _playerController.Decelerate();

        _playerController.velocity += _playerController.moveDir.x * accel * Time.deltaTime;
        _playerController.velocity = MathF.Abs(_playerController.velocity) > MaxSpeed ? MaxSpeed * _playerController.moveDir.x : _playerController.velocity;

        _playerController.Move();
    }
}
