using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocileState : IEnemyState
{
    EnemyStateMachine fsm;
    FollowPath followPath;
    
    public void Enter(EnemyStateMachine sM, FollowPath f)
    {
        Debug.Log("Entering Docile State");
        fsm = sM;
        followPath = f;    
    }

    public void Exit()
    {
        Debug.Log("Exiting Docile State");
    }

    public void Update()
    {
        if (followPath.followType != FollowPath.FollowType.BackAndForth)
        {
            Debug.Log("Setting pathfollow behaviour");
            followPath.followType = FollowPath.FollowType.BackAndForth;
        }
    }

}
