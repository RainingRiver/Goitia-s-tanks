using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingTank : AiController
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
                    ChangeState(AIState.Patrol);

                }
                break;

            case AIState.Patrol:
                // Do work
                DoPatrolState();

                // Check for transitions
                if (target != null)
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
