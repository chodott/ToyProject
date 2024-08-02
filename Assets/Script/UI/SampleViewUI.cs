using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewUI : MonoBehaviour
{
    private RawImage _rawImage;

    [SerializeField]
    private GameObject _sampleCharacterPrefab;
    [SerializeField]
    private Vector3 _characterPosition;
    private GameObject _character;

    [SerializeField]
    private Button _readyButton;
    private bool _isReady;

    private void Start()
    {
        _readyButton.GetComponent<Button>().onClick.AddListener(Ready);

        _rawImage = GetComponent<RawImage>();

        _character = Instantiate(_sampleCharacterPrefab, _characterPosition, Quaternion.identity);
        _character.GetComponent<SampleCharacter>().SetRenderTexture(_rawImage);
    }
    private void Ready()
    {
        _isReady = _isReady ? false : true;
    }
    public void ChangeCharacter(int characterNumber)
    {
        if (_isReady) return;
        _character.GetComponent<SampleCharacter>().ChangeForm(characterNumber);
    }
}
