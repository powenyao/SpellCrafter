using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour, IDamageDealer, IPooledObject
{
    public Renderer assignedRenderer;

    [HideInInspector]
    public MissileLauncher launcher;

    [SerializeField]
    private float searchRadius = 30;

    [SerializeField]
    private float damageValue = 15;

    [SerializeField]
    private Enum_Elements elementType;

    [SerializeField]
    private float moveSpeed = 15f;

    [SerializeField]
    private float rotateSpeed = 95f;

    private Rigidbody _rigidbody;
    private GameObject _target;

    private IObjectPooler pooler;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        // set color by element type
        Color color = Core.Ins.UIEffectsManager.GetColorForElement(elementType);
        assignedRenderer.material.SetColor("_BaseColor", color);

        SearchTarget();
    }

    public void SearchTarget()
    {
        RaycastHit[] hitInfo = Physics.SphereCastAll(transform.position, searchRadius, transform.forward);

        foreach (var hit in hitInfo)
        {
            GameObject hitObj = hit.transform.gameObject;
            bool isTarget = hitObj.TryGetComponent(out ShootingTarget target);
            bool isEnemy = hitObj.TryGetComponent(out EnemyStats enemy);
            if (isTarget || (isEnemy && !enemy.IsDead()))
            {
                _target = hitObj;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_target == null) return;

        _rigidbody.velocity = transform.forward * moveSpeed;

        var targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
        _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _target)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForceAtPosition(Vector3.up * 1000f, rb.position);
            }

            // return to pool
            launcher.ReturnMissileToPool(gameObject);
        }
    }

    public float GetDamageValue()
    {
        return damageValue;
    }

    public Enum_Elements GetDamageType()
    {
        return elementType;
    }

    
    public void OnObjectSpawn(IObjectPooler newPooler)
    {
        this.pooler = newPooler;
    }
}