using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BattleSceneUI : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick _horizonJoystick;
    [SerializeField]
    private VirtualJoystick _verticalJoystick;
    [SerializeField]
    private JumpButton _jumpButton;

    [SerializeField]
    private StatusUI _player1StatusUI;
    [SerializeField]
    private StatusUI _player2StatusUI;

    private PlayerController _player1;
    private PlayerController _player2;

    private Camera _mainCamera;

    [SerializeField]
    private float _cameraMinDistance = -8.0f;
    private float _cameraMaxDistance = -10.0f;

    private int _player1Score = 0;
    private int _player2Score = 0;
    [SerializeField]
    private Vector3 _player1SpawnPos = new Vector3(-5.0f, 1.0f, 0.0f);
    [SerializeField]
    private Vector3 _player2SpawnPos = new Vector3(5.0f, 1.0f, 0.0f);



    public void SetUI(PlayerController playerController1, PlayerController playerController2)
    {
        _horizonJoystick.Controller = playerController1;
        _verticalJoystick.Controller = playerController1;
        _jumpButton.Controller = playerController1;

        playerController1.HitEvent.AddListener(_player1StatusUI.UpdateStatus);
        playerController2.HitEvent.AddListener(_player2StatusUI.UpdateStatus);

        _player1 = playerController1;
        _player2 = playerController2;
        _player1.DieEvent.AddListener(SetScore);
        _player2.DieEvent.AddListener(SetScore);
        _mainCamera = Camera.main;
    }

    private void SetScore(int playerNumber)
    {
        switch(playerNumber)
        {
            case 1:
                _player1Score++;
                _player1StatusUI.UpdateScore(_player1Score);
                //StartCoroutine(_player1.Respawn(_player1SpawnPos));
                break;
            case 2:
                _player2Score++;
                _player2StatusUI.UpdateScore(_player2Score);
                //StartCoroutine(_player2.Respawn(_player2SpawnPos));
                break;
        }

        if (_player1Score >= 3 || _player2Score >= 3) QuitBattle();
    }

    private void QuitBattle()
    {
        SceneManager.LoadScene(0);
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
