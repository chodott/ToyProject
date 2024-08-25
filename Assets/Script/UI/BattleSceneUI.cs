using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneUI : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick _horizonJoystick;
    [SerializeField]
    private VirtualJoystick _verticalJoystick;
    [SerializeField]
    private JumpButton _jumpButton;
    [SerializeField]
    private StatusUI _statusUI;

    private PlayerController _player1;
    private PlayerController _player2;

    private Camera _mainCamera;

    [SerializeField]
    private float _cameraMinDistance = -8.0f;
    private float _cameraMaxDistance = -10.0f;

    public void SetUI(PlayerController playerController1, PlayerController playerController2)
    {
        _horizonJoystick.Controller = playerController1;
        _verticalJoystick.Controller = playerController1;
        _jumpButton.Controller = playerController1;

        playerController2.HitEvent.AddListener(_statusUI.UpdateStatus);

        _player1 = playerController1;
        _player2 = playerController2;

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_mainCamera == null) return;

        float xPos = (_player1.transform.position.x + _player2.transform.position.x) / 2;
        float zPos = _mainCamera.transform.position.z;
        float xDistance = -Mathf.Abs(_player1.transform.position.x - _player2.transform.position.x);
        float yDistance = -Mathf.Abs(_player1.transform.position.y - _player2.transform.position.y);
        float next_zPos = yDistance < xDistance ? yDistance : xDistance;
        next_zPos = next_zPos < _cameraMinDistance ? next_zPos : _cameraMinDistance;
        next_zPos = next_zPos < _cameraMaxDistance ? _cameraMaxDistance : next_zPos;
        zPos = Mathf.Lerp(zPos, next_zPos, Time.deltaTime);
        Vector3 cameraPosition = new Vector3(xPos, _mainCamera.transform.position.y, zPos);
        _mainCamera.transform.position = cameraPosition;
    }
}
