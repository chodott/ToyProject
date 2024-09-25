using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SelectUIManager : NetworkBehaviour
{
    [SerializeField]
    private List<SOSelectCharacter> _selectCharacterList = new();
    [SerializeField]
    private GameObject _selectUIPrefab;
    private List<SelectUI> _selectUIList = new();

    [SerializeField]
    private SampleViewUI _player1View;
    [SerializeField]
    private SampleViewUI _player2View;
    [SerializeField]
    private int _length = 3;
    [SerializeField]
    private float _limitTime = 60.0f;
    public float LimitTime
    {
        get { return _limitTime; }
    }
    readonly private float _gap = 60.0f;


    private static SelectUIManager _selectUIManager;
    public static SelectUIManager UIManager
    {
        get
        {
            if (_selectUIManager == null) _selectUIManager = FindObjectOfType<SelectUIManager>();
            return _selectUIManager;
        }
    }

    private void Start()
    {
        
        for (int i = 0; i < _selectCharacterList.Count; ++i)
        {
            //Create Select Character UI 
            GameObject selectUI = Instantiate(_selectUIPrefab);
            selectUI.transform.SetParent(gameObject.transform, false);
            _selectUIList.Add(selectUI.GetComponent<SelectUI>());
            selectUI.GetComponent<SelectUI>().SetData(_selectCharacterList[i]);
            RectTransform rt = selectUI.GetComponent<RectTransform>();

            int column = i % _length;
            int row = i / _length;
            rt.anchoredPosition = new(-_gap + column * _gap, -row * _gap + Screen.height / 4);
        }
    }


    public void SetData(SampleCharacter sample1, SampleCharacter sample2)
    {
        _player1View.SetCharacterViewRpc(sample1);
        _player2View.SetCharacterViewRpc(sample2);

        NetworkRunner runner = NetworkRunner.Instances[0];

        SampleCharacter sample = 
            runner.GetPlayerObject(runner.LocalPlayer).GetComponent<SampleCharacter>();
        sample.ChangeForm(10);
        foreach (var selectUI in _selectUIList)
        {
            selectUI.CharacterSelected.AddListener(sample.ChangeForm);
        }

    }

    public void SetData(PlayerRef player1, PlayerRef player2)
    {
        if (!HasStateAuthority) return;

        NetworkRunner runner = NetworkRunner.GetRunnerForGameObject(GameManager.Instance.gameObject);
        _player1View.SetCharacterViewRpc(runner.GetPlayerObject(player1).GetComponent<SampleCharacter>());
        _player2View.SetCharacterViewRpc(runner.GetPlayerObject(player2).GetComponent<SampleCharacter>());

        SampleCharacter sample  = 
            runner.GetPlayerObject(runner.LocalPlayer).GetComponent<SampleCharacter>();

        foreach (var selectUI in _selectUIList)
        {
            selectUI.CharacterSelected.AddListener(sample.ChangeForm);
        }

    }


    public void CheckReady(int characterNumber)
    {
        GameManager.Instance.Data = new PlayerData(0, characterNumber);
        MoveBattleScene();
    }

    public void MoveBattleScene()
    {
        
        SceneManager.LoadScene("JungleScene");
    }

    private void Update()
    {
        _limitTime -= Time.deltaTime;
        if(_limitTime < 0 ) 
        {
            MoveBattleScene();
        }
    }
}
