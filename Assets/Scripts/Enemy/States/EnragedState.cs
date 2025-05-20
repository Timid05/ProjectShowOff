using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnragedState : IEnemyState
{
    EnemyStateMachine fsm;
    EnemyStateMachine.State state = EnemyStateMachine.State.Enraged;
    FollowPath followPath;
    float oldSpeed;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        Debug.Log("Entering Enraged state");
        fsm = sM;
        followPath = f;
        fsm.SetFog(state);

        followPath.speed = fsm.GetSpeed(state);   
        oldSpeed = followPath.speed;
    }

    public void Exit()
    {
        Debug.Log("Exiting Enraged state");
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
