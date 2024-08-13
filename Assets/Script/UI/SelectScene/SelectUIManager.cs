using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SelectUIManager : MonoBehaviour
{
    [SerializeField]
    private List<SOSelectCharacter> _selectCharacterList = new();
    [SerializeField]
    private GameObject _selectUIPrefab;

    [SerializeField]
    private GameObject _player1View;
    [SerializeField]
    private GameObject _player2View;
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

    private void Start()
    {
        for (int i = 0; i < _selectCharacterList.Count; ++i)
        {
            //Create Select Character UI 
            GameObject selectUI = Instantiate(_selectUIPrefab);
            selectUI.transform.SetParent(gameObject.transform, false);
            selectUI.GetComponent<SelectUI>().SetData(_selectCharacterList[i], _player1View.GetComponent<SampleViewUI>(), _player2View.GetComponent<SampleViewUI>());
            RectTransform rt = selectUI.GetComponent<RectTransform>();

            int column = i % _length;
            int row = i / _length;
            rt.anchoredPosition = new(-_gap + column * _gap, -row * _gap + Screen.height/4);
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
