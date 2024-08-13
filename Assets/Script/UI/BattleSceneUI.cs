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
    
    public void SetUI(PlayerController playerController1, PlayerController playerController2)
    {
        _horizonJoystick.Controller = playerController1;
        _verticalJoystick.Controller = playerController1;
        _jumpButton.Controller = playerController1;

        playerController2.HitEvent.AddListener(_statusUI.UpdateStatus);
    }
}
