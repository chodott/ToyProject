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

    [Networked, OnChangedRender(nameof(FormChanged))]
    public int FormNum { get; set; }
    override public void Spawned()
    {
        _formCount = (transform.childCount-2) / 2;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeFormRpc(int num)
    {
        //if (!HasStateAuthority) return;
        FormNum = num;
    }

    public void FormChanged()
    {
        if (_curBody != null)
        {
            _curBody.SetActive(false);
            _curHead.SetActive(false);
        }

        _curBody = transform.GetChild(FormNum).gameObject;
        _curHead = transform.GetChild(FormNum + _formCount).gameObject;
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
