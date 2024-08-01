using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Idle_run_ratio = 0;

    public Vector3 AimDir  = Vector3.zero;
    public Vector3 MoveDir = Vector3.zero;
    public Rigidbody _rigidbody;
    public Animator _animator;

    [SerializeField]
    private float _friction = 5.0f;
    public float MaxSpeed { get; set; } = 3.0f;
    public float Velocity = 0.0f;

    public bool IsOnGround = false;
    public LayerMask Layer;

    private IPlayableState _idleState, _moveState;
    private StateContext _stateContext;

    private Transform _weaponEquipTransform;
    private Weapon _equippedWeapon;

    private StatManager _statManager;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        _statManager = GetComponent<StatManager>(); 
        _statManager.Die.AddListener(Respawn);

        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        _animator = GetComponent<Animator>();
        _stateContext.Transition(_idleState);

        foreach(var vjs in FindObjectsOfType<VirtualJoystick>()) 
        {
            vjs.playerController = this;
        }

        //Search Weapon Equip Object
        _weaponEquipTransform =  ReturnEquipTransform(transform);
    }

    public void Update()
    {
        AimDir.Normalize();
        MoveDir.Normalize();

        if(Input.GetButtonDown("Jump") && IsOnGround)
        {
            _rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }

        Idle_run_ratio = Math.Abs(Velocity / MaxSpeed);
        _animator.SetFloat("idle_run_ratio", Idle_run_ratio);
        _animator.SetFloat("move_direction", MathF.Sign(transform.forward.x) * MathF.Sign(AimDir.x));
    }

    private void FixedUpdate()
    {
        if(MoveDir != Vector3.zero)
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
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, Layer))
        {
            IsOnGround = true;
        }
        else IsOnGround = false;
        _animator.SetBool("OnGround", IsOnGround);
    }

    public void Decelerate()
    {
        int signOfFriction = MathF.Sign(Velocity) * -1;
        Velocity += Time.deltaTime * _friction * signOfFriction;
        Velocity = Mathf.Abs(Velocity) < 0.05f ? 0.0f : Velocity;
    }

    public void Move()
    {
        Vector3 moveVec = Vector3.zero;
        moveVec.x = Velocity;
        GetComponent<Rigidbody>().MovePosition(transform.position +  moveVec * Time.deltaTime);
    }

    public void Run(Vector2 directionVector)
    {
        _stateContext.Transition(_moveState);
        MoveDir.x = directionVector.x;
    }
    public void Stop()
    {
        _stateContext.Transition(_idleState);
        MoveDir.x = 0;
    }


    public void Turn(Vector2 directionVector)
    {
        AimDir.x = directionVector.x;
        AimDir.z = directionVector.y;

        Vector3 forwardVector = new (AimDir.x, 0, 0);
        forwardVector.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, forwardVector, 0.5f);
        Fire();
        _animator.SetFloat("aim_direction", AimDir.z);
    }

    public void EquipWeapon(GameObject weapon)
    {
        if(_equippedWeapon != null) Destroy(_equippedWeapon);

        _equippedWeapon = weapon.GetComponent<Weapon>();
        _equippedWeapon.Equipped(_weaponEquipTransform);
        _animator.SetInteger("EquippedType", _equippedWeapon.Data.Num);
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
        if (_equippedWeapon == null) return;
        _equippedWeapon.Shoot();
    }

    public void Respawn()
    {
        transform.position = new Vector3(0, 10.0f, 0);
    }
}
