using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewUI : NetworkBehaviour
{
    private RawImage _rawImage;
    [Networked] 
    public SampleCharacter _sampleCharacter { get; set; }
    private int _characterNumber;

    [SerializeField]
    private Button _readyButton;
    private bool _isReady;


    private void Awake()
    {
        _readyButton.GetComponent<Button>().onClick.AddListener(Ready);
        _rawImage = GetComponent<RawImage>();
    }

    private void Ready()
    {
        _isReady = !_isReady;
        if (_isReady == true) SelectUIManager.UIManager.CheckReady(_characterNumber);
    }
    public void ChangeCharacter(int characterNumber)
    {
        if (_isReady) return;
        _characterNumber = characterNumber;
        _sampleCharacter.GetComponent<SampleCharacter>().ChangeForm(characterNumber);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void SetCharacterViewRpc(SampleCharacter sample)
    {
        _sampleCharacter = sample;
        _sampleCharacter.SetRenderTexture(_rawImage);
    }
}
