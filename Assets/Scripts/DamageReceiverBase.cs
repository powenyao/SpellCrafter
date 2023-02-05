using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverBase : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    protected float hp = 10;

    [SerializeField]
    private AudioClip AC_OnHit;

    [SerializeField]
    protected Enum_Elements _currentElement = Enum_Elements.GrayNormal;

    [SerializeField]
    private Renderer assignedRenderer;

    [SerializeField]
    private Outline outlineRenderer;

    [SerializeField]
    private bool _hasVisualFeedback = true;

    protected float OnHitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ChangeElemental(_currentElement);
    }

    // Update is called once per frame
    void Update()
    {
        CleanDamageVisual();
    }

    protected virtual void CleanDamageVisual()
    {
        
    }

    public void ReceiveDamage(IDamageDealer damageDealer)
    {
        var damageVal = damageDealer.GetDamageValue();

        var elementType = damageDealer.GetDamageElement();
        //Dev.Log("damageVal " + damageVal);
        // Dev.Log("Damage Value: " + damageVal.ToString() + " Damage Type: " + dealer.GetDamageElement());

        if (damageVal > 0)
        {
            var effectiveDamageVal = damageVal;

            effectiveDamageVal =
                damageVal * SpellComponentReference.GetElementalDamageMultiplier(elementType, _currentElement);

            //Dev.Log("[DamageReceiverBase] Receive Damage > damageVal " + damageVal);
            //Dev.Log("[DamageReceiverBase] Receive Damage > hp " + hp);
            //Dev.Log("[DamageReceiverBase] Receive Damage > effectiveDamageVal " + effectiveDamageVal);
            var overkillValue = hp - effectiveDamageVal;
            //Dev.Log("[DamageReceiverBase] Receive Damage > overkill value " + overkillValue);

            //Powen: Why you do this to me Vinay why
            // base damage 100
            // elemental bonus 1.5
            // effective damage 150 = 100 * 1.5
            // hp 125
            // overkill damage 25
            // actual base damage dealt
            // overkilldamage / elemental bonus
            // 25/1.5 = 16.666, round down
            // 100-16 = 84
            // 84*1.5=126
            var actualDamageValueUtilized = effectiveDamageVal;
            if (damageDealer.CanPassthrough()) //if pass through
            {
                if (overkillValue < 0)
                {
                    //var actualDamageValueUtilized = damageVal - overkillValue * damageVal / effectiveDamageVal;
                    actualDamageValueUtilized = damageVal + overkillValue * damageVal / effectiveDamageVal;
                    effectiveDamageVal = hp;
                    //actualDamageValueUtilized = Mathf.CeilToInt(actualDamageValueUtilized);
                    actualDamageValueUtilized = Mathf.FloorToInt(actualDamageValueUtilized);
                    //Dev.Log("[DamageReceiverBase] Receive Damage > can overkill. actualDamageValueUtilized " + actualDamageValueUtilized);
                }
                else
                {
                    if (effectiveDamageVal < damageVal) //this is for the case where you use the wrong element
                    {
                        actualDamageValueUtilized = damageVal;
                    }       
                }
            }
            else
            {
                if (effectiveDamageVal < damageVal) //this is for the case where you use the wrong element
                {
                    actualDamageValueUtilized = damageVal;
                }    
            }

            damageDealer.DamageTakenByReceiver(actualDamageValueUtilized);
            
            hp -= effectiveDamageVal;

            ReceiveDamageVisual(damageDealer, effectiveDamageVal);

            if (hp <= 0)
            {
                Destruct();
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        {
            ReceiveDamage(dealer);
            //Dev.Log("[ShootingTarget.cs] OnCollisionEnter > " + dealer.GetDamageElement());            
        }
//        Dev.Log(other.gameObject.name);
    }

    protected virtual void OnCollisionStay(Collision other)
    {
        // if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        // {
        //     SetRendererColor(Color.yellow, "_BaseColor");
        // }
    }


    protected virtual void OnCollisionExit(Collision other)
    {
        // SetRendererColor(Color.white, "_BaseColor");
        // SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
        // //Dev.Log("sword on collision exit");
    }

    protected virtual void Destruct()
    {
    }

    protected virtual void ReceiveDamageVisual(IDamageDealer damageDealer, float effectiveDamageVal)
    {
        if (!_hasVisualFeedback)
            return;

        //Visuals
        SetRendererColor(Color.red, "_BaseColor");
        OnHitTime = Time.time;
        
        var color = Core.Ins.UIEffectsManager.GetColorForElement(damageDealer.GetDamageElement());
        Core.Ins.UIEffectsManager.RequestPopUp(this.transform, effectiveDamageVal.ToString(), color);

        //Sound
        //Core.Ins.AudioManager.PlayAudio(AC_OnHit, ENUM_Audio_Type.Sfx);
    }

    protected virtual void ChangeElemental(Enum_Elements newElement)
    {
        _currentElement = newElement;
    }

    [ContextMenu("Debug_Receive Damage 10 Electro")]
    public void Debug_ReceiveDamage()
    {
        DamageDealerForDebugging dealer = new DamageDealerForDebugging(10, Enum_Elements.PurpleElectro);
        ReceiveDamage(dealer);
    }


    protected virtual void SetRendererColor(Color color, string propertyName = null)
    {
        if (assignedRenderer)
        {
            if (propertyName != null)
                assignedRenderer.material.SetColor(propertyName, color);
            else
                assignedRenderer.material.color = color;
        }

        if (outlineRenderer)
        {
            outlineRenderer.OutlineColor = color;
        }
    }
}