using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeafTank : AiController
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
                        ChangeState(AIState.Attack);
                    }
                }
                break;

            case AIState.Attack:
                DoAttackState();
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
