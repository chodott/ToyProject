using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private int _timeSec;

    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _timeSec = ((int)SelectUIManager.UIManager.LimitTime);
        _textMeshPro.text = _timeSec.ToString();
    }
}
