using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _lerpDuration = 5f;
    private float _approachWeight = 0.5f;
    
    
    public ApproachState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
        Debug.Log("ENTER APPROACH");
        _elapsedTime = 0f;
    }

    public override void ExitState()
    {
        _elapsedTime += Time.deltaTime;
        
        environmentInteractionContext.CurrentTwoBoneIKConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentTwoBoneIKConstraint.weight, _approachWeight,
            _elapsedTime / _lerpDuration);
    }

    public override void UpdateState()
    {
    }

    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
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
