
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInterationStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext environmentInteractionContext;
    private float _movingAwayOffset = 0.005f;
    private bool _shouldReset;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,
        EnvironmentInterationStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        environmentInteractionContext = context;
    }

    protected bool CheckShouldReset()
    {
        if (_shouldReset)
        {
            environmentInteractionContext.LowestDistance = Mathf.Infinity;
            _shouldReset = false;
            return true;
        }
        
        bool isPlayerStopped = environmentInteractionContext.RigidBody.linearVelocity == Vector3.zero;
        bool isMovingAway = CheckMovingAway();
        bool isBadAngle = CheckIsBadAngle();
        bool isPlayerJumping = environmentInteractionContext.RigidBody.linearVelocity.y >= 1;
        
        if (isPlayerStopped || isMovingAway || isBadAngle || isPlayerJumping)
        {
            environmentInteractionContext.LowestDistance = Mathf.Infinity;
            return true;
        }
        
        return false;
    }
    
    private bool CheckMovingAway()
    {
        float currentDistanceToTarget = Vector3.Distance(environmentInteractionContext.RootTransform.position,
            environmentInteractionContext.ClosestPointOnColliderFromShoulder);

        bool isSearchingForNewInteraction = environmentInteractionContext.CurrentIntersectingCollider == null;
        if (isSearchingForNewInteraction)
            return false;
        
        bool isGettingCloserToTarget = currentDistanceToTarget <= environmentInteractionContext.LowestDistance;
        if (isGettingCloserToTarget)
        {
            environmentInteractionContext.LowestDistance = currentDistanceToTarget;
            return false;
        }
        
        bool isMovingAwayFromTarget = currentDistanceToTarget > environmentInteractionContext.LowestDistance + _movingAwayOffset;
        if (isMovingAwayFromTarget)
        {
            environmentInteractionContext.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }

    private bool CheckIsBadAngle()
    {
        if (environmentInteractionContext.CurrentIntersectingCollider == null)
            return false;

        Vector3 targetDirection = environmentInteractionContext.ClosestPointOnColliderFromShoulder -
                                  environmentInteractionContext.CurrentShoulderTransform.position;
        Vector3 shoulderDirection =
            environmentInteractionContext.CurrentBodySide == EnvironmentInteractionContext.EBodySide.Right
                ? environmentInteractionContext.RootTransform.right
                : -environmentInteractionContext.RootTransform.right;

        float dotProduct = Vector3.Dot(shoulderDirection, targetDirection.normalized);
        bool isBadAngel = dotProduct < 0;
        return isBadAngel;
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
            _shouldReset = true;
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

        environmentInteractionContext.CurrentIkTargetTransform.position = 
            new Vector3(offsetPosition.x, environmentInteractionContext.InteractionPointYOffset, offsetPosition.z);
    }
}
