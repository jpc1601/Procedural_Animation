
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInterationStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext environmentInteractionContext;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        environmentInteractionContext = context;
    }

    private Vector3 GetClosestPointToCollider(Collider interactionCollider, Vector3 pointToCheck)
    {
        return interactionCollider.ClosestPoint(pointToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider intersectingCollider)
    {
        Debug.Log("Start position tracking:" + intersectingCollider.name);
        if(intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && environmentInteractionContext.CurrentIntersectingCollider == null)
        {
            environmentInteractionContext.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointToCollider(intersectingCollider, environmentInteractionContext.RootTransform.position);
            environmentInteractionContext.SetCurrentSide(closestPointFromRoot);
        }
        SetIkTargetPosition();
    }
    
    protected void UpdateIkTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == environmentInteractionContext.CurrentIntersectingCollider)
            SetIkTargetPosition();
    }
    
    protected void ResetIkTargetPositionTracking(Collider intersectingCollider)
    {
        Debug.Log("Reset");
        if (intersectingCollider == environmentInteractionContext.CurrentIntersectingCollider)
        {
            environmentInteractionContext.CurrentIntersectingCollider = null;
            environmentInteractionContext.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    private void SetIkTargetPosition()
    {
        var position = environmentInteractionContext.CurrentShoulderTransform.position;
        environmentInteractionContext.ClosestPointOnColliderFromShoulder = GetClosestPointToCollider(environmentInteractionContext.CurrentIntersectingCollider
                ,new Vector3(position.x, environmentInteractionContext.CharacterShoulderHeight, position.z));

        Vector3 rayDirection = position - environmentInteractionContext.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = 0.05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;
        Vector3 offsetPosition = environmentInteractionContext.ClosestPointOnColliderFromShoulder + offset;

        environmentInteractionContext.CurrentIkTargetTransform.position = offsetPosition;
    }
}
