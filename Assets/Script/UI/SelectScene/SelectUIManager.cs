using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SelectUIManager : MonoBehaviour
{
    [SerializeField]
    private List<SOSelectCharacter> _selectCharacterList = new();
    [SerializeField]
    private GameObject _selectUIPrefab;

    [SerializeField]
    static private SampleViewUI _player1View;
    [SerializeField]
    static private SampleViewUI _player2View;
    [SerializeField]
    private int _length = 3;
    [SerializeField]
    private float _limitTime = 60.0f;
    public float LimitTime
    {
        get { return _limitTime; }
    }
    private float _gap = 60.0f;


    private static SelectUIManager _selectUIManager;
    public static SelectUIManager UIManager
    {
        get
        {
            if (_selectUIManager == null) _selectUIManager = FindObjectOfType<SelectUIManager>();
            return _selectUIManager;
        }
    }

    public void SetData(SampleCharacter sample1, SampleCharacter sample2)
    {
        _player1View.SetData(sample1);
        _player2View.SetData(sample2);

        for (int i = 0; i < _selectCharacterList.Count; ++i)
        {
            //Create Select Character UI 
            GameObject selectUI = Instantiate(_selectUIPrefab);
            selectUI.transform.SetParent(gameObject.transform, false);
            selectUI.GetComponent<SelectUI>().SetData(_selectCharacterList[i], _player1View, _player2View);
            RectTransform rt = selectUI.GetComponent<RectTransform>();

            int column = i % _length;
            int row = i / _length;
            rt.anchoredPosition = new(-_gap + column * _gap, -row * _gap + Screen.height / 4);
        }
    }

    private void Start()
    {
        
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
