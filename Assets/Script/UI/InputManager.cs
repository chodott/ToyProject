using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick _horizonJoystick;
    [SerializeField]
    private VirtualJoystick _verticalJoystick;
    [SerializeField]
    private JumpButton _jumpButton;
    private void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        _horizonJoystick.Controller = playerController;
        _verticalJoystick.Controller = playerController;
        _jumpButton.Controller = playerController;
    }
}
