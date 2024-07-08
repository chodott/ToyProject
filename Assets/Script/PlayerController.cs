using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float idle_run_ratio = 0;

    //speed

    private Vector3 aim_dir = Vector3.zero;
    public Rigidbody rigidbody;

    public float CurrentDir;
    public float velocity = 0.0f;

    enum Direction
    {
        left = -1, right = 1
    }

    private IPlayableState _idleState, _moveState, _turnState;
    private StateContext _stateContext;
    private void Start()
    {
        GameManager.Input.KeyAction -= OnKeyInput;
        GameManager.Input.KeyAction += OnKeyInput;

        rigidbody = GetComponent<Rigidbody>();

        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        _stateContext.Transition(_idleState);
    }

    public void OnKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            _stateContext.Transition(_moveState);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            _stateContext.Transition(_idleState);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _stateContext.Transition(_moveState);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _stateContext.Transition(_idleState);
        }
    }


    public void Update()
    {
        _stateContext.Update();
        aim_dir.x = Input.GetAxis("Mouse X");
        aim_dir.z = Input.GetAxis("Mouse Y");
        CurrentDir = Input.GetAxis("Horizontal");
        aim_dir.Normalize();

        if(Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }

        _stateContext.Update();
    }

    private void FixedUpdate()
    {
        if(aim_dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, aim_dir, Time.deltaTime * 5.0f);
        }
    }

}
