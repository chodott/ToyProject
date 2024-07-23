using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : MonoBehaviour
{
    private float MaxHealth = 100.0f;
    private float HealthPoint = 100.0f;
    public UnityEvent Die;

    public void TakeDamage(float damage)
    {
        HealthPoint -= damage;
        if (HealthPoint < 0) Die.Invoke();
    }


}
