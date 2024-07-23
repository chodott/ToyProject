using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float idle_run_ratio = 0;

    public Vector3 aimDir  = Vector3.zero;
    public Vector3 moveDir = Vector3.zero;
    public Rigidbody rigidbody;
    public Animator animator;

    private float Friction = 5.0f;
    public float MaxSpeed { get; set; } = 3.0f;
    public float velocity = 0.0f;

    public bool isOnGround = false;
    public LayerMask layer;

    private IPlayableState _idleState, _moveState, _turnState;
    private StateContext _stateContext;

    private Transform weaponEquipTransform;
    private Weapon equippedWeapon;

    private StatManager _statManager;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _statManager = GetComponent<StatManager>(); 
        _statManager.Die.AddListener(Respawn);

        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        animator = GetComponent<Animator>();
        _stateContext.Transition(_idleState);

        foreach(var vjs in FindObjectsOfType<VirtualJoystick>()) 
        {
            vjs.playerController = this;
        }

        //Search Weapon Equip Object
        weaponEquipTransform =  ReturnEquipTransform(transform);
    }

    public void Update()
    {
        //aimDir.x = Input.GetAxis("Mouse X");
        //aimDir.z = Input.GetAxis("Mouse Y");
        //moveDir.x = Input.GetAxis("Horizontal");
        aimDir.Normalize();
        moveDir.Normalize();

        if(Input.GetButtonDown("Jump") && isOnGround)
        {
            rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }

        idle_run_ratio = Math.Abs(velocity / MaxSpeed);
        animator.SetFloat("idle_run_ratio", idle_run_ratio);
        animator.SetFloat("move_direction", MathF.Sign(transform.forward.x) * MathF.Sign(aimDir.x));
    }

    private void FixedUpdate()
    {
        if(moveDir != Vector3.zero)
        {
            _stateContext.Transition(_moveState);
        }

        else
        {
            _stateContext.Transition(_idleState);
        }
        CheckOnGround();
        _stateContext.Update();
    }

    private void CheckOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, layer))
        {
            isOnGround = true;
        }
        else isOnGround = false;
        animator.SetBool("OnGround", isOnGround);
    }

    public void Decelerate()
    {
        int signOfFriction = MathF.Sign(velocity) * -1;
        velocity += Time.deltaTime * Friction * signOfFriction;
        velocity = Mathf.Abs(velocity) < 0.05f ? 0.0f : velocity;
    }

    public void Move()
    {
        Vector3 moveVec = Vector3.zero;
        moveVec.x = velocity;
        rigidbody.MovePosition(transform.position +  moveVec * Time.deltaTime);
    }

    public void Run(Vector2 directionVector)
    {
        _stateContext.Transition(_moveState);
        moveDir.x = directionVector.x;
    }
    public void Stop()
    {
        _stateContext.Transition(_idleState);
        moveDir.x = 0;
    }


    public void Turn(Vector2 directionVector)
    {
        aimDir.x = directionVector.x;
        aimDir.z = directionVector.y;

        Vector3 forwardVector = new Vector3(aimDir.x, 0, 0);
        forwardVector.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, forwardVector, 0.5f);
        Fire();
        animator.SetFloat("aim_direction", aimDir.z);
    }

    public void EquipWeapon(GameObject weapon)
    {
        equippedWeapon = weapon.GetComponent<Weapon>();
        equippedWeapon.Equipped(weaponEquipTransform);
        animator.SetInteger("EquippedType", equippedWeapon.Data.num);
    }

    private Transform ReturnEquipTransform(Transform parentTransform)
    {
        if (parentTransform.name == "hand_r") return parentTransform;
        Transform foundTransform;
        foreach (Transform childTransform in parentTransform)
        {
            foundTransform = ReturnEquipTransform(childTransform);
            if (foundTransform != null) return foundTransform;
        }
        return null;
    }

    private void Fire()
    {
        if (equippedWeapon == null) return;
        equippedWeapon.Shoot();
    }

    public void Respawn()
    {
        transform.position = new Vector3(0, 10.0f, 0);
    }
}
