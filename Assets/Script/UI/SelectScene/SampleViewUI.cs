using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewUI : MonoBehaviour
{
    private RawImage _rawImage;
    private SampleCharacter _sampleCharacter;
    [SerializeField]
    private Vector3 _characterPosition;
    private int _characterNumber;

    [SerializeField]
    private Button _readyButton;
    private bool _isReady;

    private void Start()
    {
        _readyButton.GetComponent<Button>().onClick.AddListener(Ready);
        _rawImage = GetComponent<RawImage>();
        DontDestroyOnLoad(gameObject);

    }

    public void SetData(SampleCharacter sample)
    {
        _sampleCharacter = sample;
        _sampleCharacter.SetRenderTexture(_rawImage);
    }

    private void Ready()
    {
        _isReady = _isReady ? false : true;
        if (_isReady == true) SelectUIManager.UIManager.CheckReady(_characterNumber);
    }
    public void ChangeCharacter(int characterNumber)
    {
        if (_isReady) return;
        _characterNumber = characterNumber;
        _sampleCharacter.GetComponent<SampleCharacter>().ChangeForm(characterNumber);
    }
}
