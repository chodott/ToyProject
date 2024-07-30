using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : MonoBehaviour
{
    private float maxHealth = 100.0f;
    private float healthPoint = 100.0f;
    public UnityEvent Die;

    public void TakeDamage(float damage)
    {
        healthPoint -= damage;
        if (healthPoint < 0) Die.Invoke();
    }


}
