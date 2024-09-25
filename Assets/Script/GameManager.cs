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
    private PlayerData _playerData = new PlayerData();
    public PlayerData Data { get { return _playerData; } set { _playerData = value; } }

    private Dictionary<string, System.Action> sceneChangeActions = new Dictionary<string, System.Action>();
    [SerializeField]
    private GameObject _characterPrefab;
    [SerializeField]
    private GameObject _battleSceneUI;
    private NetworkRunner _networkRunner;
    public NetworkRunner Runner { get; }

    void Start()
    {
        sceneChangeActions.Add("SelectScene", LoadSelectScene);
        sceneChangeActions.Add("JungleScene", LoadBattleScene);

        SceneManager.sceneLoaded += OnSceneLoad;

        _networkRunner = GetComponent<NetworkRunner>();
    }

    void LoadSelectScene()
    {
       
    }

    void LoadBattleScene()
    {
        GameObject character1 = (GameObject)Instantiate(_characterPrefab, SceneManager.GetActiveScene());
        PlayerController controller1 = character1.GetComponent<PlayerController>();
        controller1.ChangeForm(_playerData.CharacterNumber);
        character1.transform.position = new Vector3(-5.0f, 10.0f, 0.0f);

        GameObject character2 = (GameObject)Instantiate(_characterPrefab, SceneManager.GetActiveScene());
        PlayerController controller2 = character2.GetComponent<PlayerController>();
        controller2.ChangeForm(_playerData.CharacterNumber);
        character2.transform.position = new Vector3(5.0f, 10.0f, 0.0f);

        GameObject battleSceneUI = (GameObject)Instantiate(_battleSceneUI, SceneManager.GetActiveScene());
        battleSceneUI.GetComponent<BattleSceneUI>().SetUI(controller1, controller2);

        //FindObjectOfType<ComputerPlayerBT>().Target = character.transform;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneChangeActions[scene.name].Invoke();
    }
}
