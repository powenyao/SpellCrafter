using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    private Subservice_MainCharacter mainCharacter;
    private string subServiceName;

    private void OnEnable()
    {
//        Dev.Log("Player Stat OnEnable");
        Manager_Subservices.EVENT_NewSubserviceRegistered += OnNewSubserviceRegistered;
    }

    private void OnDisable()
    {
        Manager_Subservices.EVENT_NewSubserviceRegistered -= OnNewSubserviceRegistered;
    }

    private void Start()
    {
        subServiceName = nameof(Subservice_MainCharacter);
        SetupMainCharacter();
    }

    void OnNewSubserviceRegistered(string newSubservice)
    {
        //Dev.Log("PlayerStats Setup: " + newSubservice);
        if (newSubservice.Equals(subServiceName))
        {
            SetupMainCharacter();
        }
    }

    void SetupMainCharacter()
    {
        mainCharacter = (Subservice_MainCharacter)Core.Ins.Subservices.GetSubservice(subServiceName);
        if (mainCharacter)
        {
            mainCharacter.SetPlayerStat(this);
        }
    }

    protected override void Die()
    {
        base.Die();
        // kill the player
    }

    [SerializeField]
    private AudioClip ac;

    protected override void ShowDamage(IDamageDealer damageDealer)
    {
        //Core.Ins.VisualManager.PlayFlashEffect(0.1f);
        //Core.Ins.AudioManager.PlaySfx(ac);
    }
}