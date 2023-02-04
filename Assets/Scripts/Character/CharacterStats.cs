using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delegate_CharacterStatsValue(int val);

public enum ENUM_Character_Stats_Type
{
    Health,
    Damage,
    Mana
}

//TODO? convert current system to use Stats instead
//HP, MP, SP, AP, these are typically points that get expended
//Strength, dexterity, agility, constitution, intelligence, etc,
//these are typically stats that get modified and return to the base line based on some qualities
//These do not typically get expended, though in some games, when strength goes to 0, you die
//We might be able to use a class CharacterStat to represent them and access them through a data structure such as a Dictionary
//that way we can reuse code
//Though before a refactor, it's important to confirm that we can
//1) save and load the data
//2) easily modify stats for new characters
public class CharacterStat
{
    private string name;
    private int currentValue;
    private int baseValue;
    private int maxValue;
}

public class CharacterStats : MonoBehaviour, IDamageReceiver
{
    //TODO?
    #region Events

    public event Delegate_CharacterStatsValue EVENT_HealthPoints;
    public event Delegate_CharacterStatsValue EVENT_DamagePoints;
    public event Delegate_CharacterStatsValue EVENT_ManaPoints;

    #endregion

    [SerializeField]
    private int maxHealth = 100;
    private int _currentHealth;

    [SerializeField]
    private int maxManaPoints = 100;
    private int _currentMana;

    [SerializeField]
    private int baseDamage;
    private int _currentDamage;

    //TODO 
    //private Dictionary<string, int> dictionary = new Dictionary<string, int>();
    //private Dictionary<ENUM_Character_Stats_Type, CharacterStat> dictionary = new Dictionary<ENUM_Character_Stats_Type, CharacterStat>();
    protected virtual void Awake()
    {
        _currentHealth = maxHealth;
        _currentMana = maxManaPoints;
        _currentDamage = baseDamage;
    }

    public virtual void TakeMana(int manaVal)
    {
        _currentHealth -= manaVal;
        _currentMana = Mathf.Clamp(_currentMana, 0, maxManaPoints);
        PublishingValue(ENUM_Character_Stats_Type.Mana);
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    protected virtual void Die()
    {
        // Die in some way
        // this method is meant to be overwritten
    }

    public int GetValueByType(ENUM_Character_Stats_Type type)
    {
        switch (type)
        {
            case ENUM_Character_Stats_Type.Health:
                return _currentHealth;
            case ENUM_Character_Stats_Type.Mana:
                return _currentMana;
            case ENUM_Character_Stats_Type.Damage:
                return _currentDamage;
        }

        return 0;
    }

    private void PublishingValue(ENUM_Character_Stats_Type type)
    {
        switch (type)
        {
            case ENUM_Character_Stats_Type.Health:
                EVENT_HealthPoints?.Invoke(_currentHealth);
                break;
            case ENUM_Character_Stats_Type.Mana:
                EVENT_ManaPoints?.Invoke(_currentMana);
                break;
            case ENUM_Character_Stats_Type.Damage:
                EVENT_DamagePoints?.Invoke(_currentDamage);
                break;
        }
    }

    public virtual void ReceiveDamage(IDamageDealer damageDealer)
    {
        var modifiedDamageValue = damageDealer.GetDamageValue();
        
        ShowDamage(damageDealer);
        _currentHealth -= (int)modifiedDamageValue;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        PublishingValue(ENUM_Character_Stats_Type.Health);

        if (_currentHealth <= 0) Die();
    }

    protected virtual void ShowDamage(IDamageDealer damageDealer)
    {
        
    }
}