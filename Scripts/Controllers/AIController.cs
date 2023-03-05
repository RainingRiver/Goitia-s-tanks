using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AiController : Controller
{
    public enum AIState { Idle, Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost, ChooseTarget }

    public AIState currentState;

    private float LastStateChangeTime;

    public GameObject target;

    public float fleeDistance;

    public Transform[] waypoints;                                             

    public float waypointStopDistance;

    private int currentWaypoint = 0;

    public float hearingDistance;

    public float maxViewingDistance;

    public float fieldOfView;

    //Angle of the tanks field of view
    public float visionAngle;

    //How far the tank can hear
    public float hearingRadius;

    //How long the player must be out of sight for the AI to lose them
    public float lastSeenLength;

    //Last time the AI saw the player
    private float lastSeen;                                                  

    // Start is called before the first frame update
    public override void Start()
    {
        currentState= AIState.Idle;

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        MakeDecisions();

        base.Update();
    }

    public virtual void MakeDecisions()
    {
        
    }

    public virtual void ChangeState(AIState newState)
    {
        //Change the current state
        currentState = newState;

        //Save the time when we change the states
        LastStateChangeTime = Time.time;
    }

    #region States
    public void DoSeekState()
    {
        Seek(target);
    }

    protected virtual void DoAttackState()
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
    }

    protected virtual void DoChaseState()
    {
        Seek(target);
    }

    protected virtual void DoIdleState()
    {
        // Do Nothing
    }

    protected virtual void DoChooseTarget()
    {
        ChooseTarget();
    }

    protected virtual void DoPatrolState()
    {
        Patrol();
    }

    protected virtual void DoFleeState()
    {
        Flee();
    }
    #endregion States

    #region Behavior
    public void Seek(GameObject target)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(target.transform.position);

        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        pawn.RotateTowards(targetTransform.position);

        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Pawn targetPawn)
    {
        // Seek the pawn's transform!
        pawn.RotateTowards(targetPawn.transform.position);

        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Vector3 targetPosition)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }

    public void ChooseTarget()
    {
        TargetPlayerOne();
    }
    #endregion Behavior

    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Shoot()
    {
        // Tell the pawn to shoot
        pawn.Shoot();
    }

    protected void Flee()
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);

        float percentOfFleeDistance = targetDistance / fleeDistance;

        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);

        float flippedPercnetOfFlee = 1 - percentOfFleeDistance;

        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance * flippedPercnetOfFlee;

        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
    }

    #region Patrol
    protected void Patrol()
    {
        // If we have a enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            RestartPatrol();
        }
    }

    protected void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }
    #endregion Patrol

    #region Target
    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.instance != null)
        {
            // And the array of players exists
            if (GameManager.instance.players != null)
            {
                // And there are players in it
                if (GameManager.instance.players.Count > 0)
                {
                    //Then target the gameObject of the pawn of the first player controller in the list
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
        }
    }

    protected bool IsHasTarget()
    {
        // return true if we have a target, false if we don't
        return (target != null);
    }

    protected void TargetNearestTank()
    {
        // Get a list of all the tanks (pawns)
        Pawn[] allTanks = FindObjectsOfType<Pawn>();

        // Assume that the first tank is closest
        Pawn closestTank = allTanks[0];
        float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

        // Iterate through them one at a time
        foreach (Pawn tank in allTanks)
        {
            // If this one is closer than the closest
            if (Vector3.Distance(pawn.transform.position, tank.transform.position) <= closestTankDistance)
            {
                // It is the closest
                closestTank = tank;
                closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
            }
        }

        // Target the closest tank
        target = closestTank.gameObject;
    }
    #endregion Target

    public bool CanHear(GameObject target)
    {
        // Check if we actually have a target
        if (target == null)
        {
            Debug.Log("NO TARGET HEADASS");
            return false;
        }
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == false)
        {
            return false;
        }

        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }

        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;

        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            Debug.Log("can be heared");
            return true;
        }
        else
        {
            Debug.Log("hear me out");
            // Otherwise, we are too far away to hear them
            return false;
        }
    }

    public bool CanSee(GameObject target)
    {
        
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - transform.position;

        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        // if that angle is less than our field of view
        if (angleToTarget > fieldOfView)
        {
            return false;
        }

        if (!IsDistanceLessThan(target,maxViewingDistance)) 
        {
            return false;
        }

        Collider targetCollider = target.GetComponent<Collider>();

        Ray ray = new Ray(transform.position, agentToTargetVector);
        RaycastHit hitInfo;

        //If we hit nothing, return false
        if (!Physics.Raycast(ray, out hitInfo, maxViewingDistance))
        {
            return false;
        }

        // if our raycast hits nothing, we can't see them
        if (!Physics.Raycast(transform.position, agentToTargetVector, maxViewingDistance))
        {
            return false;
        }

        // If our raycast hits them first, then we can see them
        if (hitInfo.collider == targetCollider)
        {
            lastSeen = Time.time + lastSeenLength;
            return true;
        }

        // otherwise, we hit something else
        return false;
    }

}