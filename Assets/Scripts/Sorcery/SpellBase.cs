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
        isCasted = true;
    }

    public virtual void Cast(Transform targetTransform)
    {
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
        List<Enum_SpellComponents_Effects> listEffect = composition.GetEffects();
        foreach (var e in listEffect)
        {
            if (e == Enum_SpellComponents_Effects.Widen)
            {
                this.transform.localScale = this.transform.localScale * 2;
            }
        }
    }
}