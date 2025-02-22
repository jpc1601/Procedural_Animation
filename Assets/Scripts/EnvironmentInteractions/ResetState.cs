using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _resetDuration = 2f;
    
    public ResetState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState eState) : base(context, eState)
    {
        EnvironmentInteractionContext environmentInteractionContext = context;
    }

    public override void EnterState()
    {
        Debug.Log("ENTER RESET");
        _elapsedTime = 0f;
        environmentInteractionContext.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        environmentInteractionContext.CurrentIntersectingCollider = null;
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
        bool isMoving = environmentInteractionContext.RigidBody.linearVelocity != Vector3.zero;
        if(isMoving && _elapsedTime >= _resetDuration)
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Search;
        
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
