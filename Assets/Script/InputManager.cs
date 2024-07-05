using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //앞서 작성한 singleton 클래스를 상속하는 방법도 있었지만,
    //GameManager 멤버 변수로 선언했다 -> 어차피 싱글턴이 보장된다.

    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false) return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
