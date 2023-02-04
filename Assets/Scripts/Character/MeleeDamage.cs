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

    public Enum_Elements GetDamageType()
    {
        return element;
    }
}