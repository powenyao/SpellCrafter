using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour, IDamageDealer
{
    [SerializeField]
    private float DamageValue = 5;

    [SerializeField]
    private Enum_Elements element = Enum_Elements.GrayNormal;

    public MeleeDamage(float damageValue)
    {
        DamageValue = damageValue;
    }
    public float GetDamageValue()
    {
        return DamageValue;
    }

    public Enum_Elements GetDamageElement()
    {
        return element;
    }

    public void DamageTakenByReceiver(float actualDamageValueUtilized)
    {
        throw new System.NotImplementedException();
    }

    public bool CanPassthrough()
    {
        return true;
    }
}