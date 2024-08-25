using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    protected virtual void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        if(playerController != null )
        {
            playerController.TakeDamage(Damage);
        }
    }

    private void FixedUpdate()
    {
        Vector3 positionVec = transform.position;
        positionVec.z = 0f;
        transform.position = positionVec;
    }
}
