using UnityEngine;

//public partial class SpellBase : XrosGrabInteractableSubscriber_Base, IDamageDealer
public partial class SpellBase : MonoBehaviour, IDamageDealer
{
    // params about BEHAVIOR
    //[SerializeField] protected Enum_SpellBehaviors behaviorType = Enum_SpellBehaviors.Path;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 95f;

    [SerializeField]
    private Rigidbody _rigidbody;
    
    public virtual void SearchTarget()
    {
        
    }

    //public void ChangeBehavior(Enum_SpellBehaviors newBehavior)
    //{
    //    behaviorType = newBehavior;
    //}

    protected void TrackingTarget(Vector3 direction, GameObject target = null)
    {
        if (target == null)
        {
            StraightMove(direction);
            return;
        }

        _rigidbody.velocity = transform.forward * moveSpeed;
        
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed));
    }

    protected void StraightMove(Vector3 direction)
    {
        _rigidbody.velocity = direction * moveSpeed;
    }

    protected void SetRigidBodyForParabola()
    {
        _rigidbody.velocity = transform.forward * moveSpeed;
    }
}