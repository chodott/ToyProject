using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class SelectUIManager : MonoBehaviour
{
    [SerializeField]
    private List<SOSelectCharacter> _selectCharacterList = new();
    [SerializeField]
    private GameObject _selectUIPrefab;
    [SerializeField]
    private GameObject _sampleCharacterPrefab;
    private float _gap = 100.0f;
    [SerializeField]
    private Vector3 _characterPosition;
    private GameObject _character;
    private GameObject _characterView;

    private void Start()
    {
        _character = Instantiate(_sampleCharacterPrefab, _characterPosition, Quaternion.identity);
        _characterView = new GameObject("RawImage");
        _character.GetComponent<SampleCharacter>().SetRenderTexture(_characterView.AddComponent<RawImage>());
        _characterView.transform.SetParent(transform, false);
        _characterView.GetComponent<RawImage>().rectTransform.localScale *= 2;

        for(int i=0; i<_selectCharacterList.Count; ++i) 
        {
            //Create Select Character UI 
            GameObject selectUI = Instantiate(_selectUIPrefab);
            selectUI.transform.SetParent(gameObject.transform, false);
            selectUI.GetComponent<SelectUI>().SetData(_selectCharacterList[i], _character.GetComponent<SampleCharacter>());
            RectTransform rt = selectUI.GetComponent<RectTransform>();
            rt.anchoredPosition = new(_gap * i, 0); 
        }
    }

}
