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

    protected List<Enum_SpellComponents_Effects> listEffect;

    [SerializeField]
    private SpellBaseVisualization _visualization;
    
    protected bool isCasted = false;
    //track this variable
    protected float _timeSinceCast = 0f;
    
    protected bool isCompleted = false;

    public float GetDamageValue()
    {
        return damageValue;
    }

    public Enum_Elements GetDamageType()
    {
        return elementType;
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
        isCompleted = true;
    }
    
    public virtual void Process()
    {
        
    }

    public void SetupComposition(SpellComposition composition)
    {
        listEffect = composition.GetEffects();
        foreach (var e in listEffect)
        {
            switch(e)
            {
                case Enum_SpellComponents_Effects.None:
                    break;
                case Enum_SpellComponents_Effects.Pull:
                    break;
                case Enum_SpellComponents_Effects.Widen:
                    this.transform.localScale = this.transform.localScale * 2;
                    break;
                case Enum_SpellComponents_Effects.Concentrate:
                    break;
                case Enum_SpellComponents_Effects.SpeedUp:
                    moveSpeed = moveSpeed * 2f;
                    break;
                case Enum_SpellComponents_Effects.AoE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}