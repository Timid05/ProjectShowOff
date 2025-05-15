using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    FollowPath followPath;
    public EnemyStateMachine(FollowPath f)
    {
        followPath = f;
    }

    public enum State { Docile, Enraged, Aggressive }
    State currenStatename;
    private Dictionary<State, IEnemyState> states = new Dictionary<State, IEnemyState>();
    public IEnemyState currentState;

    public void SetStartState(State state)
    {
        if (states[state] != null)
        {
            currentState = states[state];
            currenStatename = state;
        }
        else
        {
            Debug.LogError("State machine does not contain a script for state: " + state);
        }
        currentState.Enter(this, followPath);
    }

    public void SetState(State state)
    {
        if (state == currenStatename) return;
        
        Debug.Log("Setting state to " + state);
        currentState?.Exit();
        
        if (states[state] != null)
        {
            currentState = states[state];
            currenStatename = state;
        }
        else
        {
            Debug.LogError("State machine does not contain a script for state: " + state);
        }
        currentState.Enter(this, followPath);
    }

    public void AddState(State stateName, IEnemyState stateScript)
    {
        states[stateName] = stateScript;
    }

    public IEnemyState GetState(State stateName)
    {
        return states[stateName];
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

}
