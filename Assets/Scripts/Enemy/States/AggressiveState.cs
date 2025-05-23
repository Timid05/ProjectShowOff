using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveState : IEnemyState
{
    EnemyStateMachine fsm;
    EnemyStateMachine.State state = EnemyStateMachine.State.Aggressive;
    FollowPath followPath;
    float oldSpeed;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        fsm = sM;
        followPath = f;

        followPath.navmeshAgent.speed = fsm.GetSpeed(state);
    }

    public void Exit()
    {
       
    }

    public void Update()
    {
        if (oldSpeed != fsm.GetSpeed(state))
        {
            followPath.navmeshAgent.speed = fsm.GetSpeed(state);
            oldSpeed = fsm.GetSpeed(state);
        }

        if (followPath.followType != FollowPath.FollowType.Target)
        {
            followPath.followType = FollowPath.FollowType.Target;
        }
    }
}
