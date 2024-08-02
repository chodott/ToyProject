using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCharacter : MonoBehaviour
{
    private GameObject _curBody;
    private GameObject _curHead;
    private Animator _animator;
    [SerializeField]
    private Camera _selfCamera;
    private int _formCount;
    private bool _isReady = false;

    private void Start()
    {
        _formCount = (transform.childCount-2) / 2;
    }
    public void ChangeForm(int num)
    {
        if (_isReady) return;

        if(_curBody != null)
        {
            _curBody.SetActive(false);
            _curHead.SetActive(false);
        }

        _curBody = transform.GetChild(num).gameObject;
        _curHead = transform.GetChild(num + _formCount).gameObject;
        _curBody.SetActive(true); 
        _curHead.SetActive(true);
    }

    public void SetRenderTexture(RawImage rawImage)
    {
        RenderTexture renderTexture = new RenderTexture(500, 500, 10);
        _selfCamera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }
}
