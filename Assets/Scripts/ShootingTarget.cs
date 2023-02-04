using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingTarget : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    private float hp = 15;

    [SerializeField]
    private Renderer assignedRenderer;

    [SerializeField]
    private AudioClip AC_OnHit;

    [SerializeField]
    private Enum_Elements _currentElement = Enum_Elements.GrayNormal;

    [SerializeField]
    private List<Enum_SpellComponents_Effects> _listSpellComponents = new List<Enum_SpellComponents_Effects>();

    void Start()
    {
        ChangeElemental(_currentElement);
    }
    
    void OnCollisionEnter(Collision other)
    {
        //Dev.Log("[ShootingTarget.cs] OnCollisionEnter");
        if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        {
            assignedRenderer.material.SetColor("_BaseColor", Color.red);
            //Dev.Log("[ShootingTarget.cs] OnCollisionEnter > " + dealer.GetDamageType());            
        }
//        Dev.Log(other.gameObject.name);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageDealer>(out IDamageDealer dealer))
        {
            assignedRenderer.material.SetColor("_BaseColor", Color.yellow);        
        }
    }

    private void OnCollisionExit(Collision other)
    {
        assignedRenderer.material.SetColor("_BaseColor", Color.white);
        assignedRenderer.material.color = Core.Ins.UIEffectsManager.GetColorForElement(_currentElement);
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
        var damageType = damageDealer.GetDamageType();
        //Dev.Log("damageVal " + damageVal);
        // Dev.Log("Damage Value: " + damageVal.ToString() + " Damage Type: " + dealer.GetDamageType());

        if (damageVal > 0)
        {
            if (damageType == _currentElement && damageType != Enum_Elements.GrayNormal)
            {
                damageVal = damageVal * ElementalDamageMultiplier;
            }
            

            //Visuals
            var color = Core.Ins.UIEffectsManager.GetColorForElement(damageDealer.GetDamageType());
            Core.Ins.UIEffectsManager.RequestPopUp(this.transform, damageVal.ToString(), color);

            //Sound
            //Core.Ins.AudioManager.PlayAudio(AC_OnHit, ENUM_Audio_Type.Sfx);

            hp -= damageVal;

            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    

    public void ChangeElemental(Enum_Elements newElement)
    {
        _currentElement = newElement;
        assignedRenderer.material.color = Core.Ins.UIEffectsManager.GetColorForElement(_currentElement);
    }

    [ContextMenu("Debug_Receive Damage 10 Electro")]
    public void Debug_ReceiveDamage()
    {
        DamageDealerForDebugging dealer = new DamageDealerForDebugging(10, Enum_Elements.PurpleElectro);
        ReceiveDamage(dealer);
    }
}