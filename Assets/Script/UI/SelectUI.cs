using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    private SOSelectCharacter _characterData;
    private Button _button;
    private Image _thumbnail;
    private SampleCharacter _sampleCharacter;

    public void SetData(SOSelectCharacter characterData, SampleCharacter sampleCharacter)
    {
        _characterData = characterData;
        _sampleCharacter = sampleCharacter;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _thumbnail = GetComponent<Image>();

        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        _sampleCharacter.ChangeForm(_characterData.Number);
    }
}
