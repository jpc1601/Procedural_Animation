using UnityEngine;

public class SearchState : EnvironmentInteractionState
{
    public float approachDistanceThreshold = 2f;
    
    public SearchState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
        Debug.Log("ENTER SEARCH");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }

    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isCloseToTarget = Vector3.Distance(environmentInteractionContext.ClosestPointOnColliderFromShoulder, environmentInteractionContext.RootTransform.position) < approachDistanceThreshold;
        bool isClosestPointOnColliderValid = environmentInteractionContext.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        // Debug.Log(isCloseToTarget);
        // Debug.Log("== " + isClosestPointOnColliderValid);
        if (isClosestPointOnColliderValid && isCloseToTarget)
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Approach;
        
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
