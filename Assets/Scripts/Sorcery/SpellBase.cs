using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public partial class SpellBase : XrosGrabInteractableSubscriber_Base, IDamageDealer
public partial class SpellBase : MonoBehaviour, IDamageDealer
{
    [SerializeField] private float damageValue = 5;

    // params about FORM
    [SerializeField] private Enum_Elements elementType;

    [SerializeField]
    private SpellBaseVisualization _visualization;
    
    protected bool isCasted = false;
    //track this variable
    protected float _timeSinceCast = 0f;
    
    protected bool isCompleted = false;

    protected SpellComposition _composition;
    
    public float GetDamageValue()
    {
        return damageValue;
    }

    public Enum_Elements GetDamageElement()
    {
        return elementType;
    }

    public virtual void DamageTakenByReceiver(float actualDamageValueUtilized)
    {
//        Dev.Log("[SpellBase.cs] DamageTakenByReceiver " +actualDamageValueUtilized);
        
        if (_composition.GetEffects().Contains(Enum_SpellComponents_Effects.PassThrough))
        {
//            Dev.Log("[SpellBase.cs] DamageTakenByReceiver Before" + damageValue);
            //damageValue -= Mathf.CeilToInt(actualDamageValueUtilized);
            damageValue -= actualDamageValueUtilized;
//            Dev.Log("[SpellBase.cs] DamageTakenByReceiver After" + damageValue);
            if (damageValue <= 0)
            {
                damageValue = 0;
                Complete();
            }
            
            //Dev.Log("damageValue " + damageValue);
        }
    }

    public virtual bool CanPassthrough()
    {
        return _composition.GetEffects().Contains(Enum_SpellComponents_Effects.PassThrough);
    }

    public void ChangeElement(Enum_Elements element)
    {
        elementType = element;
        
        _visualization.ChangeElement(elementType);
    }

    public virtual void Cast(GameObject target = null)
    {
        _timeSinceCast = 0;
        isCasted = true;
    }

    public virtual void Cast(Transform targetTransform)
    {
        _timeSinceCast = 0;
        isCasted = true;
    }

    public virtual void OnComplete()
    {
        
    }

    public virtual void Complete()
    {
        //TODO release payload
        
        
        var sfx = Instantiate (Resources.Load ("BalloonPopExplosion") as GameObject);
        sfx.transform.position = this.transform.position;
        
        isCompleted = true;
        //Dev.Log("isCompleted " + isCompleted);
    }
    
    public virtual void Process()
    {
        
    }

    public void SetupComposition(SpellComposition composition)
    {
        _composition = composition;
        
        List<Enum_SpellComponents_Effects> listEffect = composition.GetEffects();
        foreach (var e in listEffect)
        {
            switch(e)
            {
                case Enum_SpellComponents_Effects.None:
                    break;
                case Enum_SpellComponents_Effects.Pull:
                    break;
                case Enum_SpellComponents_Effects.Widen:
                    this.transform.localScale *= SpellComponentReference.Widen_ScaleMultiplier;
                    break;
                case Enum_SpellComponents_Effects.Concentrate:
                    this.damageValue *= SpellComponentReference.Concentrate_DamageMultiplier;
                    break;
                case Enum_SpellComponents_Effects.SpeedUp:
                    moveSpeed *= SpellComponentReference.Speedup_Multiplier;
                    break;
                case Enum_SpellComponents_Effects.AoE:
                    break;
                case Enum_SpellComponents_Effects.PassThrough:
                    break;
                default:
                    Dev.Log("[SpellBase] SetupComposition > " + e.ToString());
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
    }
}