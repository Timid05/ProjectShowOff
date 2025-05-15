using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnragedState : IEnemyState
{
    EnemyStateMachine fsm;
    FollowPath followPath;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        fsm = sM;
        followPath = f;

        followPath.speed += 6;
        Debug.Log("Entering Enraged state");
    }

    public void Exit()
    {
        Debug.Log("Exiting Enraged state");
    }

    public void Update()
    {

        if (followPath.followType != FollowPath.FollowType.Target)
        {
            Debug.Log("Changing pathfollow behaviour");
            followPath.followType = FollowPath.FollowType.Target;
        }
        
    }
}
