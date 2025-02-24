using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _resetDuration = 2f;
    private float _lerpDuration = 10f;
    private float _rotationSpeed = 500f;

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
        environmentInteractionContext.InteractionPointYOffset = Mathf.Lerp(
            environmentInteractionContext.InteractionPointYOffset, environmentInteractionContext.ColliderCenterY,
            _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentTwoBoneIKConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentTwoBoneIKConstraint.weight, 0,
            _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentMultiRotationConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentMultiRotationConstraint.weight, 0,
            _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentIkTargetTransform.localPosition = Vector3.Lerp(
            environmentInteractionContext.CurrentIkTargetTransform.localPosition,
            environmentInteractionContext.CurrentOriginalTargetPosition, _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(
            environmentInteractionContext.CurrentIkTargetTransform.rotation,
            environmentInteractionContext.OriginalTargetRotation, _rotationSpeed * Time.deltaTime);
    }

    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isMoving = environmentInteractionContext.RigidBody.linearVelocity != Vector3.zero;
        if (isMoving && _elapsedTime >= _resetDuration)
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