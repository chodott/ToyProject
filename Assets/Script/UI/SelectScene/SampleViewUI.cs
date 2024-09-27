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
    [Networked]
    private bool IsReady { get; set; }


    public override void Spawned()
    {
        _readyButton.GetComponent<Button>().onClick.AddListener(Ready);
        _rawImage = GetComponent<RawImage>();
    }

    private void Ready()
    {
        if (IsReady) return;
        //Disable SelectButton Only LocalPlayer
        SelectUIManager.UIManager.TurnOffSelectButton();

        //Disable ReadyButton All Player
        ReadyRpc();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void ReadyRpc()
    {
        IsReady = true;
        _readyButton.interactable = false;
        SelectUIManager.UIManager.CheckReady(_characterNumber);
    }

    public void SetCharacterView(SampleCharacter sample)
    {
        _sampleCharacter = sample;
        _sampleCharacter.SetRenderTexture(_rawImage);
    }
}
