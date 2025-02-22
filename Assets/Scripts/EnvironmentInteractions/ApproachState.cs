using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    public ApproachState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
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
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }
}
