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
    //Character Select
    [SerializeField]
    private List<SOSelectCharacter> _selectCharacterList = new();
    [SerializeField]
    private GameObject _selectUIPrefab;
    private List<SelectUI> _selectUIList = new();

    [SerializeField]
    private SampleViewUI _player1View;
    [SerializeField]
    private SampleViewUI _player2View;
    private bool IsReady1Player = false;

    //Timer
    [SerializeField]
    private TimerUI _limitTimer;
    [SerializeField]
    private float _timeLimit = 30.0f;
    public float LimitTime { get; set; }
    
    //UI 
    [SerializeField]
    private int _length = 3;
    readonly private float _gap = 60.0f;

    ////Function
    private static SelectUIManager _selectUIManager;
    public static SelectUIManager UIManager
    {
        get
        {
            if (_selectUIManager == null) _selectUIManager = FindObjectOfType<SelectUIManager>();
            return _selectUIManager;
        }
    }

    override public void Spawned()
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

        _limitTimer.EndTimerEvent.AddListener(MoveBattleScene);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void SetDataRpc(NetworkObject player1, NetworkObject player2)
    {
        _player1View.SetCharacterView(player1.GetComponent<SampleCharacter>());
        _player2View.SetCharacterView(player2.GetComponent<SampleCharacter>());

        SampleCharacter sample =
        Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<SampleCharacter>();

        foreach (var selectUI in _selectUIList)
        {
            selectUI.CharacterSelected.AddListener(sample.ChangeFormRpc);
        }

        _limitTimer.TurnOn(_timeLimit);

    }

    public void CheckReady(int characterNumber)
    {
        if(IsReady1Player)
        {
            MoveBattleScene();
        }
        IsReady1Player = true;
    }

    public void TurnOffSelectButton()
    {
        foreach (var selectUI in _selectUIList)
        {
            selectUI.TurnOff();
        }
    }

    public void MoveBattleScene()
    {
        if (!Runner.IsSceneAuthority) return;
        Runner.LoadScene("JungleScene", LoadSceneMode.Single);
    }
}