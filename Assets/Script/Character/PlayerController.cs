using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Idle_run_ratio = 0;

    public Vector3 AimDir  = Vector3.zero;
    public Vector3 MoveDir = Vector3.zero;
    private Rigidbody _rigidbody;
    private Animator _animator;

    //Form
    private GameObject _curBody;
    private GameObject _curHead;
    private int _formCount = 20;

   [SerializeField]
    private float _friction = 5.0f;
    public float MaxSpeed { get; set; } = 3.0f;
    public float Velocity = 0.0f;
    private bool _bKnockOut = false;

    public bool IsOnGround = false;
    public LayerMask _layer = 1 << 0;     //Default

    private IPlayableState _idleState, _moveState;
    private StateContext _stateContext;

    //Weapon
    [SerializeField]
    private GameObject _defaultWeaponPrefab;
    private Transform _weaponEquipTransform;
    private Weapon _equippedWeapon;

    //Status
    [SerializeField]
    private const float DamageLimit = 5.0f;
    private float _receivedDamage = 0.0f;
   
    [Serializable]
    public class floatEvent : UnityEvent<float> { }
    public floatEvent HitEvent;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;

        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        _animator = GetComponent<Animator>();
        _stateContext.Transition(_idleState);

        //Search Weapon Equip Object
        _weaponEquipTransform = ReturnEquipTransform(transform);
        EquipWeapon(Instantiate(_defaultWeaponPrefab, _weaponEquipTransform));
    }

    public void Update()
    {
        AimDir.Normalize();
        MoveDir.Normalize();

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
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, _layer))
        {
            IsOnGround = true;
        }
        else IsOnGround = false;
        _animator.SetBool("OnGround", IsOnGround);
    }

    public void Decelerate()
    {
        if (_bKnockOut) return;
        int signOfFriction = MathF.Sign(Velocity) * -1;
        Velocity += Time.deltaTime * _friction * signOfFriction;
        Velocity = Mathf.Abs(Velocity) < 0.05f ? 0.0f : Velocity;
    }

    public void Jump()
    {
        if (IsOnGround)
        {
            _rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }
    }

    public void Move()
    {
        if (_bKnockOut) return;
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
        if (Mathf.Abs(AimDir.x) <= 0.01f) forwardVector.x = 0.1f;
        forwardVector.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, forwardVector, 1.0f);
        Fire();
        _animator.SetFloat("aim_direction", AimDir.z);
    }

    public void EquipWeapon(GameObject weapon)
    {
        if(_equippedWeapon != null) Destroy(_equippedWeapon.gameObject);

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

    public void Fire()
    {
        if (_equippedWeapon == null) return;
        _equippedWeapon.Shoot();
    }

    IEnumerator Respawn()
    {
        yield return  new WaitForSeconds(2.0f);
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
        transform.position = new Vector3(0, 10.0f, 0);
        transform.forward = new Vector3(1, 0, 0);

        _stateContext.Transition(_idleState);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.inertiaTensor = Vector3.one;
        _rigidbody.inertiaTensorRotation = Quaternion.identity;

        _weaponEquipTransform = ReturnEquipTransform(transform);
        EquipWeapon(Instantiate(_defaultWeaponPrefab, _weaponEquipTransform));

        _receivedDamage = 0;
        HitEvent.Invoke(_receivedDamage);
    }

    public void ChangeForm(int num)
    {
        if (_curBody != null)
        {
            _curBody.SetActive(false);
            _curHead.SetActive(false);
        }

        _curBody = transform.GetChild(num).gameObject;
        _curHead = transform.GetChild(num + _formCount).gameObject;
        _curBody.SetActive(true);
        _curHead.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        _receivedDamage += damage;
        _receivedDamage = _receivedDamage>=DamageLimit ? DamageLimit : _receivedDamage;
        HitEvent.Invoke(_receivedDamage / DamageLimit);
        if (_receivedDamage >= DamageLimit) KnockOut();
    }

    public void KnockOut()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _bKnockOut = true;
        _rigidbody.AddForce(Vector3.up * 2000.0f);
        StartCoroutine(Respawn());
    }
}
