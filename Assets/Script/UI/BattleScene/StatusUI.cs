using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _statusTmp;
    [SerializeField]
    private TextMeshProUGUI _scoreTmp;
    private float _targetDamagePoint = 0.0f;
    private float _curDamagePoint = 0.0f;
    private float _scoreUpTime = 0.0f;

    private void Awake()
    {
        _slider = transform.GetComponent<Slider>();
    }

    public void UpdateStatus(float damagePer)
    {
        _targetDamagePoint = damagePer;
        _statusTmp.text = ((int)(damagePer * 100)).ToString() + "%";
        _scoreUpTime = 0.0f;
    }

    public void UpdateScore(int newScore)
    {
        _scoreTmp.text = newScore.ToString();
    }

    private void Update()
    {
        _scoreUpTime += Time.deltaTime;
        _curDamagePoint = Mathf.Lerp(_curDamagePoint, _targetDamagePoint, _scoreUpTime);
        _slider.value = _curDamagePoint;
    }


}
