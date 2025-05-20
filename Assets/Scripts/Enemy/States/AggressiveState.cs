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
        Debug.Log("Entering aggressive state");
        fsm = sM;
        followPath = f;

        followPath.speed = fsm.GetSpeed(state);
        fsm.SetFog(state);
    }

    public void Exit()
    {
        Debug.Log("Exiting aggressive state");
    }

    public void Update()
    {
        if (oldSpeed != fsm.GetSpeed(state))
        {
            followPath.speed = fsm.GetSpeed(state);
            oldSpeed = fsm.GetSpeed(state);
        }

        if (followPath.followType != FollowPath.FollowType.Target)
        {
            followPath.followType = FollowPath.FollowType.Target;
        }
    }
}
