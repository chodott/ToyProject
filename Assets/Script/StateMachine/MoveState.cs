using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : MonoBehaviour, IPlayableState
{
    private PlayerController _playerController;
    private float accel = 3.0f;
    public float MaxSpeed { get; set; } = 5.0f;
    public Vector3 forward_vec = Vector3.zero;


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

        float speed = Math.Abs(_playerController.velocity);
        if (speed < MaxSpeed)
        {
            int signOfMove =_playerController.CurrentDir > 0 ? 1 : -1;
            _playerController.velocity += Time.deltaTime * accel * signOfMove;
            if (Math.Abs(_playerController.velocity) > MaxSpeed) _playerController.velocity = MaxSpeed * signOfMove;
        }
        print(_playerController.velocity);
        forward_vec.x = _playerController.velocity;
        _playerController.rigidbody.MovePosition( forward_vec * Time.deltaTime + _playerController.transform.position);
    }
}
