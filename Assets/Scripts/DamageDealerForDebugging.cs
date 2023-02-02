public class DamageDealerForDebugging : IDamageDealer
{
    private float _damageValue = 10f;
    private Enum_Elements _element = Enum_Elements.GrayNormal;

    public float GetDamageValue()
    {
        return _damageValue;
    }

    public Enum_Elements GetDamageType()
    {
        return _element;
    }

    //Constructors
    public DamageDealerForDebugging()
    {
    }

    public DamageDealerForDebugging(Enum_Elements newElement)
    {
        _element = newElement;
    }

    public DamageDealerForDebugging(float newDamageValue, Enum_Elements newElement = Enum_Elements.GrayNormal)
    {
        _damageValue = newDamageValue;
        _element = newElement;
    }
}