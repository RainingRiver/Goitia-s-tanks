using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    public Transform firepointTransform;
    private TankPawn tankPawn;
    private float lastTimeShoot;

    // Start is called before the first frame update
    public override void Start()
    {
        tankPawn = GetComponent<TankPawn>();

        lastTimeShoot= Time.time;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        if (Time.time > lastTimeShoot + tankPawn.fireCoolDown)
        {
            //instatiate out projectile
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);

            //get the damageOnHit
            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

            if (doh != null)
            {
                doh.damgeDone = damageDone;
                doh.owner = GetComponent<Pawn>();
            }

            Rigidbody rb = newShell.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firepointTransform.forward * fireForce);
            }

            lastTimeShoot = Time.time;

            Destroy(newShell, lifeSpan);
        }
    }
}
