using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�ռ� �ۼ��� singleton Ŭ������ ����ϴ� ����� �־�����,
    //GameManager ��� ������ �����ߴ� -> ������ �̱����� ����ȴ�.

    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false) return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
