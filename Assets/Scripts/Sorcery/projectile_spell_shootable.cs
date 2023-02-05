using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        else if (_composition.GetTracking() != Enum_SpellComponents_Tracking.None)
        {
            SearchTarget();
        }

        _withoutTarget = _targetObj == null;
        _shootForward = transform.forward;

        if (_composition.GetTracking() == Enum_SpellComponents_Tracking.None &&
            _composition.GetPath() == Enum_SpellComponents_Path.Parabola)
        {
            SetRigidBodyForParabola();
        }

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
        //switch (behaviorType)
        //{
        //    case Enum_SpellBehaviors.Tracking:
        //        TrackingTarget(_shootForward, _targetObj);
        //        break;
        //    case Enum_SpellBehaviors.Path:
        //    {
        //        switch (_composition.GetPath())
        //        {
        //            case Enum_SpellComponents_Path.Parabola:
        //            {
        //                break;
        //            }
        //            default:
        //            {
        //                StraightMove(_shootForward);
        //                break;
        //            }
        //        }

        //        break;
        //    }
        //    default:
        //        StraightMove(_shootForward);
        //        break;
        //}

        switch (_composition.GetTracking())
        {
            case Enum_SpellComponents_Tracking.None:
            {
                switch (_composition.GetPath())
                {
                    case Enum_SpellComponents_Path.Parabola:
                    {
                        break;
                    }
                    default:
                    {
                        StraightMove(_shootForward);
                        break;
                    }
                }

                break;
            }
            default:
            {
                TrackingTarget(_shootForward, _targetObj);

                break;
            }
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
            if (_composition.GetEffects().Contains(Enum_SpellComponents_Effects.PassThrough))
            {
//                Dev.Log("Has passthrough");
            }
            else
            {
                Complete();    
            }
        }
        else
        {
            //Dev.Log("[Projectile_spell_shootable] OnCollisionEnter > Didn't hit damageReceiver, hit " + Dev.GetPath(collision.gameObject.transform));
            
            //Complete();
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

    private RaycastHit[] _hitInfo = new RaycastHit[10];
    private bool gotTarget = false;

    public override void SearchTarget()
    {
        Physics.SphereCastNonAlloc(transform.position, searchRadius, transform.forward, _hitInfo);

        List<RaycastHit> list = _hitInfo.ToList().OrderBy(o => GetTargetDistance(o)).ToList();

        foreach (var hit in list)
        {
            if (hit.transform == null)
                continue;
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.TryGetComponent(out ITarget receiver))
            {
                if (!gotTarget)
                {
                    _targetObj = hitObj;
                    gotTarget = true;
                }

                Dev.Log("Target: " + hit.transform.name + "; Distance: " + GetTargetDistance(hit));

                //break;
            }
        }

        gotTarget = false;
        Dev.Log("=============================================");
    }

    private float GetTargetDistance(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            return Vector3.Distance(hit.transform.position, transform.position);
        }
        else
        {
            return 0;
        }
    }
}