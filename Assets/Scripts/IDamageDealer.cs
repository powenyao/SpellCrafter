/// <summary>
/// Interface start with I
/// This IDamageDealer interface should be used by any script that can do damage. E.g. swords and bullets
///
/// Those scripts will then need to implement DamageValue which can be used to determine how much damage is dealt.
/// </summary>
public interface IDamageDealer
{
    //We might want to add damage source and damage type in the future
    //damage type e.g. fire, physical, ice
    //damage source to indicate who or what dealt the damage
    float GetDamageValue();

    Enum_Elements GetDamageType();
}

public interface IDamageReceiver
{
    void ReceiveDamage(IDamageDealer damageDealer);
}

public interface IDestructible
{
    void Destruct();
}