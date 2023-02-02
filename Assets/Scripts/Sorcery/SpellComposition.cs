using System.Collections.Generic;

public class SpellComposition
{
    private Enum_SpellShapes shape;
    private Enum_Elements element;
    // private List<Enum_SpellEffects> effectsList;
    
    private bool _isShapeReady = false;
    private bool _isElementReady = false;

    public SpellComposition()
    {
        shape = Enum_SpellShapes.Sphere;
        element = Enum_Elements.GrayNormal;
        // effectsList = new List<Enum_SpellEffects>();
    }

    public void SetElement(Enum_Elements e)
    {
        element = e;
        _isElementReady = true;
    }

    public void SetShape(Enum_SpellShapes s)
    {
        shape = s;
        _isShapeReady = true;
    }
    
    public Enum_Elements GetElement()
    {
        return element;
    }

    public Enum_SpellShapes GetShape()
    {
        return shape;
    }
    
    public bool IsPrepped()
    {
        return _isShapeReady && _isElementReady;
    }
}