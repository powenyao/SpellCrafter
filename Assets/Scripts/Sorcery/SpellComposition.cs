using System;
using System.Collections.Generic;

/// <summary>
/// SpellComposition is intended to be a data class to store the spell that is being prepared
/// It'd typically be composed of element + shape + effect(s)
/// </summary>
public class SpellComposition
{
    //GDD
    //https://docs.google.com/document/d/1bpgvqLUAh2KFm38x1B09lKZv8ziRAPY3x0u9LvHuLNY/edit#heading=h.6k2zcbc348z
    
    //FORM
    private Enum_SpellShapes _shape;
    private Enum_Elements _element;
    
    //BEHAVIOR
    private Enum_SpellComponents_Tracking _tracking;
    private Enum_SpellComponents_Path _path;
    
    //EFFECTS
    private List<Enum_SpellComponents_Effects> _listSpellComponentEffects = new List<Enum_SpellComponents_Effects>();
    
    // private List<Enum_SpellEffects> effectsList;
    
    //Shape and Element must be ready before the spell can first take shape
    private bool _isShapeReady = false;
    private bool _isElementReady = false;

    #region constructors
    /// <summary>
    /// Default spell is of a sphere shape and with gray color to reflect the neutral elemental effect.
    /// </summary>
    public SpellComposition()
    {
        _shape = Enum_SpellShapes.Sphere;
        _element = Enum_Elements.GrayNormal;
        // effectsList = new List<Enum_SpellEffects>();
    }

    public SpellComposition(Enum_SpellShapes newShape, Enum_Elements newElement)
    {
        _shape = newShape;
        _element = newElement;
    }

    #endregion constructors
    
    public void SetElement(Enum_Elements e)
    {
        _element = e;
        _isElementReady = true;
    }

    public void SetShape(Enum_SpellShapes s)
    {
        _shape = s;
        _isShapeReady = true;
    }
    
    public Enum_Elements GetElement()
    {
        return _element;
    }

    public Enum_SpellShapes GetShape()
    {
        return _shape;
    }
    
    public bool IsPrepped()
    {
        return _isShapeReady && _isElementReady;
    }

    public void AddSpellComponent(Enum_SpellComponentCategories category, string component)
    {
        switch(category)
        {
            case Enum_SpellComponentCategories.None:
                break;
            case Enum_SpellComponentCategories.Element:
                break;
            case Enum_SpellComponentCategories.Shape:
                break;
            case Enum_SpellComponentCategories.Effects:
                //Convert string to the appropriate effect
                Enum_SpellComponents_Effects effects = Enum_SpellComponents_Effects.None;
                if (Enum.TryParse<Enum_SpellComponents_Effects>(component, out effects))
                {
                    if (Enum.IsDefined(typeof(Enum_SpellComponents_Effects), effects))
                    {
                        _listSpellComponentEffects.Add(effects);    
                    }
                    else
                    {
                        Dev.LogWarning("[SpellComposition.cs] AddSpellComponent > " + component +" is not an underlying value of the enum " + nameof(Enum_SpellComponents_Effects));
                    }
                }
                else
                {
                    Dev.LogWarning("[SpellComposition.cs] AddSpellComponent > " + component +" is not a member of the enum " + nameof(Enum_SpellComponents_Effects));
                }
                break;
            case Enum_SpellComponentCategories.Origin:
                break;
            case Enum_SpellComponentCategories.Target:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(category), category, null);
        }
    }

    public List<Enum_SpellComponents_Effects> GetEffects()
    {
        return _listSpellComponentEffects;
    }
}