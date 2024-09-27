using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    private SOSelectCharacter _characterData;
    private Button _button;
    public SOSelectCharacter CharacterData
    {
        get { return _characterData; }
        set { _characterData = value; }
    }
    private Image _thumbnail;

    public UnityEvent<int> CharacterSelected;

    private void Start()
    {
        _button = GetComponent<Button>();
        _thumbnail = GetComponent<Image>();

        _button.onClick.AddListener(SelectCharacter);
    }

    public void SetData(SOSelectCharacter characterData)
    {
        _characterData = characterData;
    }

    public void TurnOff()
    {
        _button.interactable = false;
    }

     void SelectCharacter()
    {
        CharacterSelected.Invoke(_characterData.Number);
    }
}
