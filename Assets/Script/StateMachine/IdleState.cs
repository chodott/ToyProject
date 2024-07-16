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

    public void Update()
    {
        if (_playerController == null) return;

        _playerController.Decelerate();
        _playerController.Move();

        _playerController.idle_run_ratio = Mathf.Lerp(_playerController.idle_run_ratio, 0, 5 * Time.deltaTime);
        Animator animator = _playerController.GetComponent<Animator>();
        if (animator == null) return;
        animator.SetFloat("idle_run_ratio", _playerController.idle_run_ratio);
        animator.Play("OnGround");
    }
}
