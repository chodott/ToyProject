using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : MonoBehaviour
{
    private float _maxHealth = 100.0f;
    private float _healthPoint = 100.0f;
    public UnityEvent Die;

    public void TakeDamage(float damage)
    {
        _healthPoint -= damage;
        if (_healthPoint < 0) Die.Invoke();
    }


}
