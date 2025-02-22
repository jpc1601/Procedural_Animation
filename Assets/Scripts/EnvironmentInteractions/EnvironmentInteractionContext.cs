using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext 
{
    public enum EBodySide
    {
        Right,
        Left
    }
    
    private TwoBoneIKConstraint _leftArmTwoBoneIKConstraint;
    private TwoBoneIKConstraint _rightArmTwoBoneIKConstraint;
    private MultiRotationConstraint _leftArmMultiRotationConstraint;
    private MultiRotationConstraint _rightArmMultiRotationConstraint;
    private Rigidbody _rigidBody;
    private CapsuleCollider _capsuleCollider;
    private Transform _rootTransform;

    public EnvironmentInteractionContext(
        TwoBoneIKConstraint leftArmTwoBoneIKConstraint,
        TwoBoneIKConstraint rightArmTwoBoneIKConstraint,
        MultiRotationConstraint leftArmMultiRotationConstraint,
        MultiRotationConstraint rightArmMultiRotationConstraint,
        Rigidbody rigidbody, CapsuleCollider capsuleCollider,
        Transform rootTransform)
    {
        _leftArmTwoBoneIKConstraint = leftArmTwoBoneIKConstraint;
        _rightArmTwoBoneIKConstraint = rightArmTwoBoneIKConstraint;
        _leftArmMultiRotationConstraint = leftArmMultiRotationConstraint;
        _rightArmMultiRotationConstraint = rightArmMultiRotationConstraint;
        _rigidBody = rigidbody;
        _capsuleCollider = capsuleCollider;
        _rootTransform = rootTransform;

        CharacterShoulderHeight = _leftArmTwoBoneIKConstraint.data.root.parent.position.y;
    }
    
    public TwoBoneIKConstraint LeftArmTwoBoneIKConstraint => _leftArmTwoBoneIKConstraint;
    public TwoBoneIKConstraint RightArmTwoBoneIKConstraint => _rightArmTwoBoneIKConstraint;
    public MultiRotationConstraint LeftArmMultiRotationConstraint => _leftArmMultiRotationConstraint;
    public MultiRotationConstraint RightArmMultiRotationConstraint => _rightArmMultiRotationConstraint;
    public Rigidbody RigidBody => _rigidBody;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;
    public Transform RootTransform => _rootTransform;

    public Collider CurrentIntersectingCollider { get; set; }
    public TwoBoneIKConstraint CurrentTwoBoneIKConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public Transform CurrentIkTargetTransform { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float CharacterShoulderHeight { get; set; }
    public EBodySide CurrentBodySide { get; private set; }

    public void SetCurrentSide(Vector3 pointToCheck)
    {
        Vector3 leftShoulder = _leftArmTwoBoneIKConstraint.data.root.position;
        Vector3 rightShoulder = _rightArmTwoBoneIKConstraint.data.root.position;

        bool isLeftSide = Vector3.Distance(pointToCheck, leftShoulder) < Vector3.Distance(pointToCheck, rightShoulder);
        if (isLeftSide)
        {
            Debug.Log("LEFT");
            CurrentBodySide = EBodySide.Left;
            CurrentTwoBoneIKConstraint = LeftArmTwoBoneIKConstraint;
            CurrentMultiRotationConstraint = LeftArmMultiRotationConstraint;
        }
        else
        {
            Debug.Log("RIGHT");
            CurrentBodySide = EBodySide.Right;
            CurrentTwoBoneIKConstraint = RightArmTwoBoneIKConstraint;
            CurrentMultiRotationConstraint = RightArmMultiRotationConstraint;
        }

        CurrentIkTargetTransform = CurrentTwoBoneIKConstraint.data.target.transform;
        CurrentShoulderTransform = CurrentTwoBoneIKConstraint.data.root.parent;
    }
}
