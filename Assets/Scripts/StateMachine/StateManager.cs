using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new();
    protected BaseState<EState> currentState;

    private bool isTransitioningState;
    
    private void Start()
    {
        currentState.EnterState();
    }

    private void Update()
    {
        var nextStateKey = currentState.GetNextState();
        if(!isTransitioningState && nextStateKey.Equals(currentState.StateKey))
            currentState.UpdateState();
        else if(!isTransitioningState)
            TransitionToState(nextStateKey);
    }

    private void TransitionToState(EState stateKey)
    {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = States[stateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
}
