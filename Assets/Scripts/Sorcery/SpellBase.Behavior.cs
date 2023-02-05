using UnityEngine;

//public partial class SpellBase : XrosGrabInteractableSubscriber_Base, IDamageDealer
public partial class SpellBase : MonoBehaviour, IDamageDealer
{
    // params about BEHAVIOR
    //[SerializeField] protected Enum_SpellBehaviors behaviorType = Enum_SpellBehaviors.Path;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 95f;
    [SerializeField] protected float rotateInit;
    [SerializeField] protected float rotatePace;
    protected float rotateValue = 0f;
    [SerializeField] protected float enableHomingTime;
    [SerializeField] protected float forceFactor;
    protected float enableHomingTimer;

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

        switch (_composition.GetTracking())
        {
            case Enum_SpellComponents_Tracking.Partial:
            {
                rotateValue = rotateSpeed;

                var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateValue));

                break;
            }
            case Enum_SpellComponents_Tracking.Full:
            {
                //rotateValue += rotatePace;
                if (enableHomingTimer > enableHomingTime)
                {
                    rotateValue = 180f;

                    var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateValue));
                }
                else
                {
                    StraightMove(transform.forward);
                }

                break;
            }
        }

        enableHomingTimer += Time.deltaTime;
    }

    protected void StraightMove(Vector3 direction)
    {
        _rigidbody.velocity = direction * moveSpeed;
    }

    protected void SetRigidBodyForParabola()
    {
        _rigidbody.velocity = transform.forward * moveSpeed;
    }

    protected void SetCurvedMode()
    {
        _rigidbody.velocity = transform.forward * moveSpeed;
        _rigidbody.useGravity = false;
    }    

    protected void CurvedMove()
    {
        _rigidbody.AddForce(-transform.up * forceFactor);
    }
}