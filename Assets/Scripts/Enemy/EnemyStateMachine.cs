using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine
{
    FollowPath followPath;
    Dictionary<State, float> stateSpeeds;

    public EnemyStateMachine(FollowPath f, Dictionary<State, float> speeds)
    {
        followPath = f;
        stateSpeeds = speeds;
    }
    
    public enum State { Docile, Aggressive, Enraged }
    public State currentStateName;
    State oldStateName;
    private Dictionary<State, IEnemyState> states = new Dictionary<State, IEnemyState>();
    public IEnemyState currentState;

    public void SetStartState(State state)
    {
        if (states[state] != null)
        {
            currentState = states[state];
            currentStateName = state;
            oldStateName = state;
            EnemiesInfo.OnStateChange?.Invoke(currentStateName);
        }
        else
        {
            Debug.LogError("State machine does not contain a script for state: " + state);
        }
        currentState.Enter(this, followPath);
    }

    public float GetSpeed(State state)
    {
        return stateSpeeds[state];
    }
    public void UpdateSpeeds(Dictionary<State, float> speeds)
    {
        stateSpeeds = speeds;
        Debug.Log("fsm speeds updated");
    }

    public void SetState(State state)
    {
        if (state == currentStateName) return;

        Debug.Log("Setting state to " + state);
        currentState?.Exit();

        if (states[state] != null)
        {
            currentState = states[state];
            currentStateName = state;
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

        if (currentStateName != oldStateName)
        {
            EnemiesInfo.OnStateChange?.Invoke(currentStateName);
            oldStateName = currentStateName;
        }
    }

}
