using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}

public class GameManager : Singleton<GameManager>
{
    InputManager _input = new InputManager();
    public static InputManager Input { get { return instance._input; } }

    void Start()
    {
        
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
