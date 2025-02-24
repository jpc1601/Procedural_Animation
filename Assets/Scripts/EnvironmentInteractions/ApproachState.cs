using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _lerpDuration = 5f;
    private float _approachDuration = 2f;
    private float _approachWeight = 0.5f;
    private float _approachRotationWeight = 0.75f;
    private float _rotationSpeed = 500f;
    private float _riseDistanceThreshold = 0.5f;
    
    
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
    }

    public override void UpdateState()
    {
        Quaternion expectedGroundRotation =
            Quaternion.LookRotation(-Vector3.up, environmentInteractionContext.RootTransform.forward);
        
        _elapsedTime += Time.deltaTime;

        environmentInteractionContext.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(
            environmentInteractionContext.CurrentIkTargetTransform.rotation, expectedGroundRotation, 
            _elapsedTime / _rotationSpeed);
        
        environmentInteractionContext.CurrentMultiRotationConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentMultiRotationConstraint.weight, _approachRotationWeight,
            _elapsedTime / _lerpDuration);
        
        environmentInteractionContext.CurrentTwoBoneIKConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentTwoBoneIKConstraint.weight, _approachWeight,
            _elapsedTime / _lerpDuration);
    }

    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
        if(isOverStateLifeDuration)
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Reset;
        
        bool isWithinArmReach = Vector3.Distance(environmentInteractionContext.ClosestPointOnColliderFromShoulder,
            environmentInteractionContext.CurrentShoulderTransform.position) < _riseDistanceThreshold;
        bool isClosestPointOnColliderValid = environmentInteractionContext.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;
        if (isWithinArmReach && isClosestPointOnColliderValid)
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Rise;
        
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
