using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damgeDone;
    public Pawn owner;

    public void OnTriggerEnter(Collider other)
    {
        Health otherHealth= other.gameObject.GetComponent<Health>();

        if(otherHealth != null)
        {
            otherHealth.TakeDamage(damgeDone, owner);
        }

        Destroy(gameObject);
    }
}

