using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IPlayableState
{
    private PlayerController _playerController;

    private float friction = 3.0f;
    public Vector3 forward_vec = Vector3.zero;
    public void Handle(PlayerController playerController)
    {
        if (_playerController == null) _playerController = playerController;
    }

    public void Update()
    {
        if (_playerController == null) return;

        _playerController.idle_run_ratio = Mathf.Lerp(_playerController.idle_run_ratio, 0, 5 * Time.deltaTime);
        Animator animator = _playerController.GetComponent<Animator>();
        if (animator == null) return;
        animator.SetFloat("idle_run_ratio", _playerController.idle_run_ratio);
        animator.Play("OnGround");

        int signOfFriction = _playerController.velocity < 0 ? 1 : -1;
        _playerController.velocity += signOfFriction * Time.deltaTime * friction;
        _playerController.velocity = Mathf.Abs(_playerController.velocity) <= 0.5f ? 0 : _playerController.velocity;
        forward_vec.x = _playerController.velocity;
        _playerController.rigidbody.MovePosition(forward_vec * Time.deltaTime + _playerController.transform.position);
    }
}
