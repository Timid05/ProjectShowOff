using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveState : IEnemyState
{
    EnemyStateMachine fsm;
    FollowPath followPath;

    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        Debug.Log("Entering aggressive state");
        fsm = sM;
        followPath = f;

    }

    public void Exit()
    {
        Debug.Log("Exiting aggressive state");
    }

    public void Update()
    {
        if (followPath.followType != FollowPath.FollowType.Target)
        {
            Debug.Log("setting pathfollow behaviour");
            followPath.followType = FollowPath.FollowType.Target;
        }
    }
}
