using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour
{
    private PlayerController _playerController;
    public PlayerController Controller;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PushButton);
    }

    public void PushButton()
    {
        Controller.Jump();
    }
}
