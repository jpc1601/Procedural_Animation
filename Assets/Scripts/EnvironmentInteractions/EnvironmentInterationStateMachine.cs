using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInterationStateMachine : StateManager<EnvironmentInterationStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint leftArmTwoBoneIKConstraint;
    [SerializeField] private TwoBoneIKConstraint rightArmTwoBoneIKConstraint;
    [SerializeField] private MultiRotationConstraint leftArmMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint rightArmMultiRotationConstraint;
    // [SerializeField] private CharacterController characterController;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody rigidbody;

    private EnvironmentInteractionContext environmentInteractionContext;
    private void Awake()
    {
        environmentInteractionContext = new EnvironmentInteractionContext(
            leftArmTwoBoneIKConstraint, rightArmTwoBoneIKConstraint, leftArmMultiRotationConstraint,
            rightArmMultiRotationConstraint, rigidbody, capsuleCollider, transform.root);
        
        InitializeState();
        ConstructInteractiveCollider();
    }

    private void InitializeState()
    {
        States.Add(EEnvironmentInteractionState.Search, new SearchState(environmentInteractionContext, EEnvironmentInteractionState.Search));    
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(environmentInteractionContext, EEnvironmentInteractionState.Approach));    
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(environmentInteractionContext, EEnvironmentInteractionState.Rise));    
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(environmentInteractionContext, EEnvironmentInteractionState.Touch));    
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(environmentInteractionContext, EEnvironmentInteractionState.Reset));

        currentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructInteractiveCollider()
    {
        float wingSize = capsuleCollider.height;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingSize, wingSize, wingSize);
        
        var center = capsuleCollider.center;
        boxCollider.center = new Vector3(center.x, center.y + wingSize * 0.25f, center.z + wingSize * 0.5f);
        boxCollider.isTrigger = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (environmentInteractionContext != null 
            && environmentInteractionContext.ClosestPointOnColliderFromShoulder != null
            && environmentInteractionContext.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity)
            Gizmos.DrawSphere(environmentInteractionContext.ClosestPointOnColliderFromShoulder, 0.03f);
    }
}
