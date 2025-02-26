using UnityEngine;

public class TouchState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _resetThreshold = 0.5f;
    
    public TouchState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
        _elapsedTime = 0;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (_elapsedTime > _resetThreshold || CheckShouldReset())
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Reset;
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetPositionTracking(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        UpdateIkTargetPosition(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }
}
