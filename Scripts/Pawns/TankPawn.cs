using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    public Shooter shooter;
    public GameObject shellPrefab;
    public float fireForce;
    public float damageDone;
    public float lifeSpan;
    public float fireCoolDown = 1.5f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForward()
    {
        if (mover != null)
        {
            mover.Move(transform.forward, moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveForward()!");
        }
    }

    public override void MoveBackward()
    {
        if (mover != null)
        {
            mover.Move(transform.forward, -moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveBackward()!");
        }
    }

    public override void RotateClockwise()
    {
        if (mover != null)
        {
            mover.Rotate(turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateClockwise()!");
        }
    }

    public override void RotateCounterClockwise()
    {
        if (mover != null)
        {
            mover.Rotate(-turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateCounterClockwise()!");
        }

    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        Vector3 vectorToTarget = targetPosition- transform.position;
        Quaternion TargetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, turnSpeed * Time.deltaTime);
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, lifeSpan);

    }
}

