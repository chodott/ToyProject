using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCharacter : NetworkBehaviour
{
    private GameObject _curBody;
    private GameObject _curHead;
    [SerializeField]
    private Camera _selfCamera;
    private int _formCount;
    private bool _isReady = false;

    [Networked, OnChangedRender(nameof(FormChanged))]
    private int _formNum { get; set; }

    private void Start()
    {
        _formCount = (transform.childCount-2) / 2;
    }
    public void ChangeForm(int num)
    {
        if (!HasStateAuthority) return;
        if (_isReady) return;
        _formNum = num;
    }

    public void FormChanged()
    {
        if (_isReady) return;

        if (_curBody != null)
        {
            _curBody.SetActive(false);
            _curHead.SetActive(false);
        }

        _curBody = transform.GetChild(_formNum).gameObject;
        _curHead = transform.GetChild(_formNum + _formCount).gameObject;
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
