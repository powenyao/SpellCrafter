using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delegate_NewMainCharacter(PlayerStats playerStats);

public class Subservice_MainCharacter : XrosSubservice
{
    [SerializeField]
    private PlayerStats player;

    public static event Delegate_NewMainCharacter EVENT_NewMainCharacter;
    
    void OnEnable()
    {
        Core.Ins.Subservices.RegisterService(nameof(Subservice_MainCharacter), this);
    }

    void OnDisable()
    {
        Core.Ins.Subservices.UnregisterService(nameof(Subservice_MainCharacter), this);
    }

    public void SetPlayerStat(PlayerStats stat)
    {
//        Dev.Log("New Player Stats");
        player = stat;
        EVENT_NewMainCharacter?.Invoke(player);
    }
    

    public PlayerStats GetPlayerStat()
    {
        return player;
    }

    public void ApplyProperty(ConsumableProperty property)
    {
        var dictionary = property.GetProperties();
        for (int i = 0; i < dictionary.Count; i++)
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}