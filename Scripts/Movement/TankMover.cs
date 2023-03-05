using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    // Variable to hold the Rigidbody Component
    private Rigidbody rb;
    private NoiseMaker nm;

    // Start is called before the first frame update
    public override void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        nm = GetComponent<NoiseMaker>();

    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
        if(nm != null ) 
        { 
            nm.volumeDistance = 10;
        }
    }

    public override void Rotate(float turnSpeed)
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        if (nm != null)
        {
            nm.volumeDistance = 10;
        }
    }
}
