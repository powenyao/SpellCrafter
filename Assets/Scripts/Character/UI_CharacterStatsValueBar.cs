using System;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
/*
public class UI_CharacterStatsValueBarForMainCharacter : UI_CharacterStatsValueBar
{
    void OnEnable()
    {
        Subservice_MainCharacter.EVENT_NewMainCharacter += SetupMC;
    }

    private void OnDisable()
    {
        Subservice_MainCharacter.EVENT_NewMainCharacter += SetupMC;
    }

    void SetupMC(PlayerStats stats)
    {
        statsController = stats;
        Setup();
    }
}
*/

public class UI_CharacterStatsValueBar : MonoBehaviour
{
    public CharacterStats statsController;

    [System.Serializable]
    public class ValueBar
    {
        public ENUM_Character_Stats_Type type;
        public Image fillingImg;
        public int maxValue;
    }

    public ValueBar bar;
    private int _curVal;

    private void Start()
    {
        if (bar != null && statsController != null)
        {
            Setup();
        }
    }

    public void Setup()
    {
        if (bar == null)
        {
            Dev.LogWarning("bar is not assigned in " + Dev.GetPath(this));
            return;
        }

        if (statsController == null)
        {
            Dev.LogWarning("statsController is not assigned in " + Dev.GetPath(this));
            return;
        }

        bar.maxValue = statsController.GetValueByType(bar.type);
        _curVal = bar.maxValue;

        switch (bar.type)
        {
            case ENUM_Character_Stats_Type.Health:
                statsController.EVENT_HealthPoints += HandleCurValueChange;
                break;
            case ENUM_Character_Stats_Type.Damage:
                statsController.EVENT_DamagePoints += HandleCurValueChange;
                break;
            case ENUM_Character_Stats_Type.Mana:
                statsController.EVENT_ManaPoints += HandleCurValueChange;
                break;
        }
    }

    private void Update()
    {
        if (bar != null)
        {
            bar.fillingImg.fillAmount = (float)_curVal / (float)bar.maxValue;
        }
    }

    private void HandleCurValueChange(int val)
    {
        _curVal = val;
    }
}