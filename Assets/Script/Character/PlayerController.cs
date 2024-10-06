using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour
{
    public float Idle_run_ratio = 0;

    [Networked, OnChangedRender(nameof(ChangedAim))]
    public Vector3 AimDir { get; set; }
    public Vector3 MoveDir = Vector3.zero;
    private Rigidbody _rigidbody;
    private NetworkRigidbody3D _networkRigidbody;
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
    private bool _bInvincible = false;

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


    private SkinnedMeshRenderer _headMeshRenderer;
    private SkinnedMeshRenderer _bodyMeshRenderer;
    static private float _emphaticBlinkIndencity = 0.4f;
    static private float _defaultBlinkIndencity = 0.2f;
    private float _targetBlinkIndencity;
    private bool _bEmphasized = false;
    private float _blinkTimer = 0.0f;
    static private float _blinkDuration = 0.3f;


    [SerializeField]
    static private float _respawnTime = 2.0f;
    [SerializeField]
    static private float _invincibleTime = 2.0f;


    public UnityEvent<float> HitEvent;
    public UnityEvent<int> DieEvent;
    private int _playerNumber = 1;
    public override void Spawned()
    {
        Runner.SetIsSimulated(Runner.GetPlayerObject(Runner.LocalPlayer), true);

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;

        //NetworkRigidbody
        _networkRigidbody = GetComponent<NetworkRigidbody3D>();
        _networkRigidbody.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _networkRigidbody.Rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;


        _stateContext = new StateContext(this);

        _idleState = new IdleState();
        _moveState = new MoveState();

        _animator = GetComponent<Animator>();
        _stateContext.Transition(_idleState);

        //Search Weapon Equip Object
        _weaponEquipTransform = ReturnEquipTransform(transform);
        EquipWeapon(Instantiate(_defaultWeaponPrefab, _weaponEquipTransform));
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;

        if (MoveDir != Vector3.zero)
        {
            _stateContext.Transition(_moveState);
        }

        else
        {
            _stateContext.Transition(_idleState);
        }
        CheckOnGround();
        _stateContext.Update(Runner.DeltaTime);

        AimDir.Normalize();
        MoveDir.Normalize();

        Idle_run_ratio = Math.Abs(Velocity / MaxSpeed);
        _animator.SetFloat("idle_run_ratio", Idle_run_ratio);
        _animator.SetFloat("move_direction", MathF.Sign(transform.forward.x) * MathF.Sign(AimDir.x));

        if (_bInvincible)
        {
            _blinkTimer += Time.deltaTime;
            float lerp = Mathf.Clamp01(_blinkTimer / _blinkDuration);
            Color emissionColor = Color.white * lerp * _targetBlinkIndencity;
            _bodyMeshRenderer.material.SetColor("_EmissionColor", emissionColor);
            _headMeshRenderer.material.SetColor("_EmissionColor", emissionColor);
        }

        Vector3 forwardVector = new (AimDir.x, 0, 0);
        if (Mathf.Abs(AimDir.x) <= 0.01f) forwardVector.x = 0.1f;
        forwardVector.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, forwardVector, 1.0f);
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
        Velocity += Runner.DeltaTime * _friction * signOfFriction;
        Velocity = Mathf.Abs(Velocity) < 0.05f ? 0.0f : Velocity;
    }

    public void Jump()
    {
        if (IsOnGround)
        {
            //NetworkRigidbody
            _networkRigidbody.Rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
        }
    }

    public void Move()
    {
        if (_bKnockOut) return;
        Vector3 moveVec = Vector3.zero;
        moveVec.x = Velocity;

        //NetworkRigidbody
        _networkRigidbody.Rigidbody.MovePosition(transform.position + moveVec * Runner.DeltaTime);
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
        AimDir = new Vector3(directionVector.x, 0.0f, directionVector.y);
        Fire();
    }

    public void ChangedAim()
    {
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

    public IEnumerator Respawn(Vector3 respawnPos)
    {
        yield return  new WaitForSeconds(_respawnTime);
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

        _bKnockOut = false;
        _receivedDamage = 0;
        HitEvent.Invoke(_receivedDamage);
        StartCoroutine(Guard());
    }

    public IEnumerator Guard()
    {
        _bInvincible = true;

        _headMeshRenderer.material.EnableKeyword("_EMISSION");
        _bodyMeshRenderer.material.EnableKeyword("_EMISSION");
        StartCoroutine(Blink());
        yield return new WaitForSeconds(_invincibleTime);
        _bInvincible = false;
        _headMeshRenderer.material.DisableKeyword("_EMISSION");
        _bodyMeshRenderer.material.DisableKeyword("_EMISSION");
    }

    public IEnumerator Blink()
    {
        if (!_bInvincible) yield return 0;

        yield return new WaitForSeconds(_blinkDuration);
        _bEmphasized = !_bEmphasized;
        _targetBlinkIndencity = _bEmphasized ? _emphaticBlinkIndencity : _defaultBlinkIndencity;
        _blinkTimer = 0.0f;
        StartCoroutine(Blink());

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

        _headMeshRenderer = _curHead.GetComponent<SkinnedMeshRenderer>();
        _bodyMeshRenderer = _curBody.GetComponent<SkinnedMeshRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (_bKnockOut) return;
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
        DieEvent.Invoke(_playerNumber);
        StartCoroutine(Respawn(new Vector3(0,1,0)));
    }
}
