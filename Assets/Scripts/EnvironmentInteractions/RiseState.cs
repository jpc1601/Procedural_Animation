using UnityEngine;

public class RiseState : EnvironmentInteractionState
{
    private float _elapsedTime;
    private float _lerpDuration = 5f;
    private float _riseWeight = 1f;
    private float _maxDistance = 0.5f;
    private float _rotaionSpeed = 1000f;
    private float _touchDistaceThreshold = 0.5f;
    private float _touchTimeThreshold = 1f;
    private Quaternion _expectedHandRotation;
    private LayerMask _interactableLayerMask = LayerMask.GetMask("Interactable");
    
    public RiseState(EnvironmentInteractionContext context,
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
        CalculateExpectedHandRotation();
        environmentInteractionContext.InteractionPointYOffset = Mathf.Lerp(
            environmentInteractionContext.InteractionPointYOffset,
            environmentInteractionContext.ClosestPointOnColliderFromShoulder.y, _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentTwoBoneIKConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentTwoBoneIKConstraint.weight,
            _riseWeight, _elapsedTime / _lerpDuration);
        
        environmentInteractionContext.CurrentMultiRotationConstraint.weight = Mathf.Lerp(
            environmentInteractionContext.CurrentMultiRotationConstraint.weight,
            _riseWeight, _elapsedTime / _lerpDuration);

        environmentInteractionContext.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(
            environmentInteractionContext.CurrentIkTargetTransform.rotation, _expectedHandRotation,
            _rotaionSpeed * Time.deltaTime);
        
        _elapsedTime += Time.deltaTime;
    }

    private void CalculateExpectedHandRotation()
    {
        Vector3 startPos = environmentInteractionContext.CurrentShoulderTransform.position;
        Vector3 endPos = environmentInteractionContext.ClosestPointOnColliderFromShoulder;
        Vector3 direction = (startPos - endPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, _maxDistance, _interactableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = -surfaceNormal;
            _expectedHandRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }
    }
    
    public override EnvironmentInterationStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (CheckShouldReset())
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Reset;
        
        if (Vector3.Distance(environmentInteractionContext.CurrentIkTargetTransform.position,
                environmentInteractionContext.ClosestPointOnColliderFromShoulder) < _touchDistaceThreshold
            && _elapsedTime >= _touchTimeThreshold)
            return EnvironmentInterationStateMachine.EEnvironmentInteractionState.Touch;
        
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
