using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
using Debug = UnityEngine.Debug;

public class projectile_spell_shootable : SpellBase
{
    [SerializeField]
    private float searchRadius = 30f;

    private Vector3 _shootForward;

    
    //time before a spell expires
    [SerializeField]
    private float terminateTime = 5f;

    private GameObject _targetObj;
    private bool _withoutTarget = true;

    void Update()
    {
        Process();
    }

    // protected override void OnSelectExited(SelectExitEventArgs args)
    // {
    //     base.OnSelectExited(args);
    //
    //     this.transform.forward = args.interactorObject.transform.forward;
    //     Cast();
    // }

    public override void Cast(GameObject target = null)
    {
        if (target)
        {
            //Aim the spell at where the controller is pointing at
            this.transform.LookAt(target.transform.position);
            _targetObj = target;
        }
        else if (behaviorType == Enum_SpellBehaviors.Tracking)
        {
            SearchTarget();
        }

        _withoutTarget = _targetObj == null;
        _shootForward = transform.forward;
        
        base.Cast(target);
        
    }

    public override void Cast(Transform targetTransform)
    {
        this.transform.LookAt(targetTransform.position);
        _shootForward = transform.forward;
        isCasted = true;
        
        base.Cast(targetTransform);
    }

    public override void Process()
    {
        //if (!_grabInteractable.isSelected && isCasted)
        if (isCasted)
        {
            Moving();

            _timeSinceCast += Time.deltaTime;
        }

        if (_withoutTarget && _timeSinceCast > terminateTime)
        {
            // Dev.Log("time up");
            Complete();
        }

        if (isCompleted)
        {
            OnComplete();
        }
    }

    private void Moving()
    {
        switch (behaviorType)
        {
            case Enum_SpellBehaviors.Tracking:
                TrackingTarget(_shootForward, _targetObj);
                break;
            case Enum_SpellBehaviors.Path:
            default:
                StraightMove(_shootForward);
                break;
        }
    }

    public override void OnComplete()
    {
        base.OnComplete();
        //TODO switch to Object Pool
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.isCasted)
        {
            return;
        }
        
        if (collision.gameObject.TryGetComponent<IDamageReceiver>(out IDamageReceiver receiver))
        {
            Complete();
        }

        //Powen: Below is Barry's Code. I don't think a tracking spell should keep going even if it already hit something else until it hit the target it wants. 
        // if (behaviorType == Enum_SpellBehaviors.Path)
        // {
        //     Complete();
        // }
        // else
        // {
        //     bool isDamageReceiver = collision.gameObject.TryGetComponent<IDamageReceiver>(out IDamageReceiver receiver);
        //     bool hitTarget = collision.gameObject == _targetObj;
        //     if (isDamageReceiver && hitTarget)
        //     {
        //         Complete();
        //     }
        // }
    }

    private RaycastHit[] _hitInfo;

    public override void SearchTarget()
    {
        Physics.SphereCastNonAlloc(transform.position, searchRadius, transform.forward, _hitInfo);

        foreach (var hit in _hitInfo)
        {
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.TryGetComponent(out IDamageReceiver receiver))
            {
                _targetObj = hitObj;
                break;
            }
        }
    }
}