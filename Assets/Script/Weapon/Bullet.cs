using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    protected virtual void OnCollisionEnter(Collision collision)
    {
        StatManager otherStat = collision.transform.GetComponent<StatManager>();
        if(otherStat != null )
        {
            otherStat.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
