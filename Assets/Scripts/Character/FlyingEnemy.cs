using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class FlyingEnemy : MonoBehaviour
{
    private Transform _target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private MeleeDamage _meleeDamage;

    [SerializeField]
    private CharacterStats _enemyStats;

    [SerializeField]
    private CharacterAnimator _characterAnimator;

    // attacking
    [SerializeField]
    private float timeBetweenAttacks;

    [SerializeField]
    private float attackRadius;

    private bool _hasAttacked;

    private Rigidbody _rigidbody;


    private void Start()
    {
        //_target = FindObjectOfType<_XrOriginGO>().gameObject.transform;
        //_target = Core.Ins.XRManager.GetXrOrigin_Xros().gameObject.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_enemyStats.GetValueByType(ENUM_Character_Stats_Type.Health) <= 0)
        {
            return;
        }

        Moving();
        _characterAnimator.PlayMoving(_rigidbody.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= attackRadius)
        {
            Attacking();
        }
        else
        {
            _characterAnimator.StopAttacking();
        }
    }

    private void Moving()
    {
        FaceTarget();
        var dir = new Vector3(Mathf.Cos(Time.time * speed), Mathf.Sin(Time.time * speed));
        _rigidbody.velocity = dir;
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void Attacking()
    {
        _characterAnimator.PlayAttacking();
        if (!_hasAttacked)
        {
            // attack player
            CharacterStats targetStats = _target.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                targetStats.ReceiveDamage(_meleeDamage);
                //targetStats.TakeDamage(_enemyStats.GetValueByType(ENUM_Character_Stats_Type.Damage));
            }

            _hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _hasAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}