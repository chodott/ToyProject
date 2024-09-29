using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
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

    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, System.Action> sceneChangeActions = new Dictionary<string, System.Action>();
    [SerializeField]
    private GameObject _battleSceneUI;

    void Start()
    {
        sceneChangeActions.Add("SelectScene", LoadSelectScene);
        sceneChangeActions.Add("JungleScene", LoadBattleScene);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void LoadSelectScene()
    {
       
    }

    void LoadBattleScene()
    {
  
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneChangeActions[scene.name].Invoke();
    }
}
