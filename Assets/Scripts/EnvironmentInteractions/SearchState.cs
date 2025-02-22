using UnityEngine;

public class SearchState : EnvironmentInteractionState
{
    public SearchState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
        Debug.Log("SEARCH START");
    }

    public override void ExitState()
    {
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
