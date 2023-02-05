using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void OnTargetDestroyedHandler(ShootingTarget target);

public class ShootingTarget : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    private float hp = 15;

    [SerializeField]
    private Renderer assignedRenderer;
    [SerializeField]
    private Outline outlineRenderer;

    [SerializeField]
    private AudioClip AC_OnHit;

    [SerializeField]
    private Enum_Elements _currentElement = Enum_Elements.GrayNormal;

    [SerializeField]
    private List<Enum_SpellComponents_Effects> _listSpellComponents = new List<Enum_SpellComponents_Effects>();

    public event OnTargetDestroyedHandler OnTargetDestroyed;

    void SetRendererColor(Color color, string name = null)
    {
        if (assignedRenderer)
        {
            if (name != null)
                assignedRenderer.material.SetColor(name, Color.red);
            else
                assignedRenderer.material.color = color;
        }
        if (outlineRenderer)
        {
            outlineRenderer.OutlineColor = color;
        }
    }

    void Start()
    {
        ChangeElemental(_currentElement);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        {
            ReceiveDamage(dealer);
            //Dev.Log("[ShootingTarget.cs] OnCollisionEnter > " + dealer.GetDamageElement());            
        }
//        Dev.Log(other.gameObject.name);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        {
            SetRendererColor(Color.yellow, "_BaseColor");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        SetRendererColor(Color.white, "_BaseColor");
        SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
        //Dev.Log("sword on collision exit");
    }

    private void ShowDamage(IDamageDealer dealer)
    {
    }

    private void Update()
    {
        // if (Input.GetKeyUp(KeyCode.I))
        // {
        //     ChangeElemental((Enum_Elements)(Random.value * 7));
        // }
    }

    //@vinay might want to move this elsewhere if we aren't using a generic bonus
    private int ElementalDamageMultiplier = 3;

    
    public void ReceiveDamage(IDamageDealer damageDealer)
    {
        var damageVal = (int)damageDealer.GetDamageValue();
        
        var damageType = damageDealer.GetDamageElement();
        //Dev.Log("damageVal " + damageVal);
        // Dev.Log("Damage Value: " + damageVal.ToString() + " Damage Type: " + dealer.GetDamageElement());

        if (damageVal > 0)
        {
            var effectiveDamageVal = damageVal;
            if (damageType == _currentElement && damageType != Enum_Elements.GrayNormal)
            {
                effectiveDamageVal = damageVal * ElementalDamageMultiplier;
            }
            
            //Check to see if effectiveDamage will kill the player
            var overkillValue = hp - effectiveDamageVal;
            
            
            if (overkillValue > 0) //not overkill
            {
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

                if (damageDealer.CanOverkill())
                {
                    damageDealer.DamageTakenByReceiver(effectiveDamageVal);
                    hp -= effectiveDamageVal;
                }
                else
                {
                    var actualDamageValueUtilized = damageVal - overkillValue * damageVal / effectiveDamageVal; 
                    damageDealer.DamageTakenByReceiver(actualDamageValueUtilized);


                    hp = 0;
                }
                
                
                
            }
            else
            {
                Destruct();
            }

            //Visuals
            SetRendererColor(Color.red, "_BaseColor");

            var color = Core.Ins.UIEffectsManager.GetColorForElement(damageDealer.GetDamageElement());
            Core.Ins.UIEffectsManager.RequestPopUp(this.transform, effectiveDamageVal.ToString(), color);

            //Sound
            //Core.Ins.AudioManager.PlayAudio(AC_OnHit, ENUM_Audio_Type.Sfx);

        }
    }

    public void Destruct()
    {
        OnTargetDestroyed?.Invoke(this);
        Destroy(this.gameObject);
    }
    

    public void ChangeElemental(Enum_Elements newElement)
    {
        _currentElement = newElement;
        SetRendererColor(Core.Ins.UIEffectsManager.GetColorForElement(_currentElement));
    }

    [ContextMenu("Debug_Receive Damage 10 Electro")]
    public void Debug_ReceiveDamage()
    {
        DamageDealerForDebugging dealer = new DamageDealerForDebugging(10, Enum_Elements.PurpleElectro);
        ReceiveDamage(dealer);
    }
}