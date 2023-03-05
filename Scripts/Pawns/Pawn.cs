using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    public Mover mover;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public abstract void MoveForward();

    public abstract void MoveBackward();

    public abstract void RotateClockwise();

    public abstract void RotateCounterClockwise();

    public abstract void Shoot();

    public abstract void RotateTowards(Vector3 targetPosition);
}
