using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AiController;
using static UnityEngine.GraphicsBuffer;

public class FleeTank : AiController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        MakeDecisions();

        base.Update();
    }

    public override void MakeDecisions()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case AIState.Idle:
                // Do work 
                DoIdleState();

                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (IsDistanceLessThan(target, maxViewingDistance))
                {
                    if (CanSee(target))
                    {
                        Debug.Log("i see you");
                        ChangeState(AIState.Flee);
                    }
                }
                break;

            case AIState.Chase:
                // Do work
                DoChaseState();

                // Check for transitions
                if (!IsDistanceLessThan(target, 100))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.Flee:
                DoFleeState();
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.ChooseTarget:
                DoChooseTarget();
                if (target != null)
                {
                    ChangeState(AIState.Idle);
                }

                break;
        }
    }
}
