using System.Collections;
using System.Collections.Generic;
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
    private PlayerData _playerData = new PlayerData();
    public PlayerData Data { get { return _playerData; } set { _playerData = value; } }

    private Dictionary<string, System.Action> sceneChangeActions = new Dictionary<string, System.Action>();
    [SerializeField]
    private GameObject _characterPrefab;

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
        GameObject character = (GameObject) Instantiate(_characterPrefab, SceneManager.GetActiveScene());
        character.GetComponent<PlayerController>().ChangeForm(_playerData.CharacterNumber);
        character.transform.position = new Vector3(-5.0f, 10.0f, 0.0f);

        FindObjectOfType<ComputerPlayerBT>().Target = character.transform;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneChangeActions[scene.name].Invoke();
    }
}
