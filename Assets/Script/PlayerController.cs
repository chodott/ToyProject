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

    public Vector3 aimDir  = Vector3.zero;
    public Vector3 moveDir = Vector3.zero;
    public Rigidbody rigidbody;

    private float Friction = 5.0f;
    public float velocity = 0.0f;

    public bool isOnGround = false;
    public LayerMask layer;

    private IPlayableState _idleState, _moveState, _turnState;
    private StateContext _stateContext;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        _stateContext.Transition(_idleState);

        foreach(var vjs in FindObjectsOfType<VirtualJoystick>()) 
        {
            vjs.playerController = this;
        }
    }

    public void Update()
    {
        //aimDir.x = Input.GetAxis("Mouse X");
        //aimDir.z = Input.GetAxis("Mouse Y");
        //moveDir.x = Input.GetAxis("Horizontal");
        aimDir.Normalize();
        moveDir.Normalize();

        _stateContext.Update();
        CheckOnGround();

        if(Input.GetButtonDown("Jump") && isOnGround)
        {
            rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }

    }

    private void FixedUpdate()
    {
        if(aimDir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 5.0f);
        }

        if(moveDir != Vector3.zero)
        {
            _stateContext.Transition(_moveState);
        }

        else
        {
            _stateContext.Transition(_idleState);
        }
    }

    private void CheckOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
        {
            isOnGround = true;
        }
        else isOnGround = false;
    }

    public void Decelerate()
    {
        int signOfFriction = MathF.Sign(velocity) * -1;
        velocity += Time.deltaTime * Friction * signOfFriction;
        velocity = Mathf.Abs(velocity) < 0.2f ? 0.0f : velocity;
    }

    public void Move()
    {
        Vector3 moveVec = Vector3.zero;
        moveVec.x = velocity;
        rigidbody.MovePosition(transform.position +  moveVec * Time.deltaTime);
    }

    public void Run()
    {
        _stateContext.Transition(_moveState);
    }
    public void Stop()
    {
        _stateContext.Transition(_idleState);
    }
}
