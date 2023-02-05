using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void OnTargetDestroyedHandler(ShootingTarget target);

public class ShootingTarget : DamageReceiverBase, ITarget
{
    public event OnTargetDestroyedHandler OnTargetDestroyed;
    
    // protected override void OnCollisionStay(Collision other)
    // {
    //     if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
    //     {
    //         SetRendererColor(Color.yellow, "_BaseColor");
    //     }
    // }
    //
    // protected override void OnCollisionExit(Collision other)
    // {
    //     //SetRendererColor(Color.white, "_BaseColor");
    //     SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
    //     //Dev.Log("sword on collision exit");
    // }

    
    // public void ReceiveDamage(IDamageDealer damageDealer)
    // {
    //     var damageVal = (int)damageDealer.GetDamageValue();
    //     
    //     var damageType = damageDealer.GetDamageElement();
    //     //Dev.Log("damageVal " + damageVal);
    //     // Dev.Log("Damage Value: " + damageVal.ToString() + " Damage Type: " + dealer.GetDamageElement());
    //
    //     if (damageVal > 0)
    //     {
    //         var effectiveDamageVal = damageVal;
    //         if (damageType == _currentElement && damageType != Enum_Elements.GrayNormal)
    //         {
    //             effectiveDamageVal = damageVal * ElementalDamageMultiplier;
    //         }
    //         
    //         //Check to see if effectiveDamage will kill the player
    //         var overkillValue = hp - effectiveDamageVal;
    //         
    //         
    //         if (overkillValue > 0) //not overkill
    //         {
    //             //Powen: Why you do this to me Vinay why
    //             // base damage 100
    //             // elemental bonus 1.5
    //             // effective damage 150 = 100 * 1.5
    //             // hp 125
    //             // overkill damage 25
    //             // actual base damage dealt
    //             // overkilldamage / elemental bonus
    //             // 25/1.5 = 16.666, round down
    //             // 100-16 = 84
    //             // 84*1.5=126
    //
    //             if (damageDealer.CanOverkill())
    //             {
    //                 damageDealer.DamageTakenByReceiver(effectiveDamageVal);
    //                 hp -= effectiveDamageVal;
    //             }
    //             else
    //             {
    //                 var actualDamageValueUtilized = damageVal - overkillValue * damageVal / effectiveDamageVal; 
    //                 damageDealer.DamageTakenByReceiver(actualDamageValueUtilized);
    //
    //
    //                 hp = 0;
    //             }
    //             
    //             
    //             
    //         }
    //         else
    //         {
    //             Destruct();
    //         }
    //
    //         //Visuals
    //         SetRendererColor(Color.red, "_BaseColor");
    //
    //         var color = Core.Ins.UIEffectsManager.GetColorForElement(damageDealer.GetDamageElement());
    //         Core.Ins.UIEffectsManager.RequestPopUp(this.transform, effectiveDamageVal.ToString(), color);
    //
    //         //Sound
    //         //Core.Ins.AudioManager.PlayAudio(AC_OnHit, ENUM_Audio_Type.Sfx);
    //
    //     }
    // }

    protected override void Destruct()
    {
        OnTargetDestroyed?.Invoke(this);
        Destroy(this.gameObject);
    }
    
    protected override void ChangeElemental(Enum_Elements newElement)
    {
        base.ChangeElemental(newElement);
        SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
    }
    [SerializeField]
    private float colorCooldown = 0.5f;

    
    protected override void CleanDamageVisual()
    {
        if (Time.time > OnHitTime + colorCooldown)
        {
            SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
        }
    }

    public GameObject GetTarget()
    {
        return this.gameObject;
    }
}