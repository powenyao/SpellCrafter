using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void Delegate_NewSpellPreparation();
public delegate void Delegate_NewSpellCast();
//public delegate void Delegate_NewSpellPreparation(string launcher);
//public delegate void Delegate_NewSpellCast(string launcher);

public class Subservice_Sorcery : XrosSubservice
{
    public static event Delegate_NewSpellCast EVENT_NewSpellCast;
    public static event Delegate_NewSpellPreparation EVENT_NewSpellPreparation;

    private Dictionary<Enum_SpellShapes, GameObject> _spellsList = new Dictionary<Enum_SpellShapes, GameObject>();
    private Dictionary<Enum_SpellShapes, Sprite> shapeSpriteDictionary = new Dictionary<Enum_SpellShapes, Sprite>();
    private Dictionary<Enum_Elements, Sprite> elementSpriteDictionary = new Dictionary<Enum_Elements, Sprite>();

    private Dictionary<string, SpellComposition> _launcherList = new Dictionary<string, SpellComposition>();
    
    //Use prefabs or Resources.Load? Started spells with prefabs, but require assignment
    //Used Resources.Load for element and shape sprites, but that is easy to break
    [SerializeField]
    private GameObject PF_Basic;

    [SerializeField]
    private GameObject PF_Horizontal;

    [SerializeField]
    private GameObject PF_Cross;
    
    [SerializeField]
    private GameObject PF_Wall;
    
    [SerializeField]
    private GameObject PF_Spikes;

    //SpellComposition is intended to be a data class to store the spell that is being prepared
    //It'd typically be composed of element + shape + effect(s)
    private SpellComposition _currentComposition;

    #region Setup
    void OnEnable()
    {
        Core.Ins.Subservices.RegisterService(nameof(Subservice_Sorcery), this);

        Setup();
    }

    void OnDisable()
    {
        Core.Ins.Subservices.UnregisterService(nameof(Subservice_Sorcery), this);
    }
    
    void Setup()
    {
        SetupSpells();
        SetupSprites();
        SetupDebug();
    }
    
    void SetupSpells()
    {
        //Setup Spells
        _spellsList.Add(Enum_SpellShapes.Sphere, PF_Basic);
        _spellsList.Add(Enum_SpellShapes.Horizontal, PF_Horizontal);
        _spellsList.Add(Enum_SpellShapes.Cross, PF_Cross);
        _spellsList.Add(Enum_SpellShapes.Wall, PF_Wall);
        _spellsList.Add(Enum_SpellShapes.Spikes, PF_Spikes);
        
        // init the data class to store the prepared spell
        _currentComposition = new SpellComposition();
    }
    void SetupSprites()
    {
        //Note: Prone to breaking from renamed files
        elementSpriteDictionary.Add(Enum_Elements.GrayNormal, Resources.Load<Sprite>("SP_Element_Neutral"));
        elementSpriteDictionary.Add(Enum_Elements.OrangePyro, Resources.Load<Sprite>("SP_Element_Pyro"));
        elementSpriteDictionary.Add(Enum_Elements.GreenDendro, Resources.Load<Sprite>("SP_Element_Dendro"));
        elementSpriteDictionary.Add(Enum_Elements.YellowGeo, Resources.Load<Sprite>("SP_Element_Geo"));
        elementSpriteDictionary.Add(Enum_Elements.TealAnemo, Resources.Load<Sprite>("SP_Element_Anemo"));
        elementSpriteDictionary.Add(Enum_Elements.PurpleElectro, Resources.Load<Sprite>("SP_Element_Electro"));
        elementSpriteDictionary.Add(Enum_Elements.BlueHydro, Resources.Load<Sprite>("SP_Element_Hydro"));
        
        shapeSpriteDictionary.Add(Enum_SpellShapes.Sphere, Resources.Load<Sprite>("SP_Shape_Sphere"));
        shapeSpriteDictionary.Add(Enum_SpellShapes.Cross, Resources.Load<Sprite>("SP_Shape_Cross"));
        shapeSpriteDictionary.Add(Enum_SpellShapes.Horizontal, Resources.Load<Sprite>("SP_Shape_Bar"));
        shapeSpriteDictionary.Add(Enum_SpellShapes.Spikes, Resources.Load<Sprite>("SP_Shape_Spikes"));
        shapeSpriteDictionary.Add(Enum_SpellShapes.Wall, Resources.Load<Sprite>("SP_Shape_Wall"));
    }

    void SetupDebug()
    {
        //Debug
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Null", () => this.PrepSpell(Enum_Elements.GrayNormal) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Fire", () => this.PrepSpell(Enum_Elements.OrangePyro) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Wood", () => this.PrepSpell(Enum_Elements.GreenDendro) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Earth", () => this.PrepSpell(Enum_Elements.YellowGeo) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Wind", () => this.PrepSpell(Enum_Elements.TealAnemo) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Electric", () => this.PrepSpell(Enum_Elements.PurpleElectro) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Water", () => this.PrepSpell(Enum_Elements.BlueHydro) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Sphere", () => this.PrepSpell(Enum_SpellShapes.Sphere) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Wall", () => this.PrepSpell(Enum_SpellShapes.Wall) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Cross", () => this.PrepSpell(Enum_SpellShapes.Cross) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Bar", () => this.PrepSpell(Enum_SpellShapes.Horizontal) );
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Spikes", () => this.PrepSpell(Enum_SpellShapes.Spikes) );
    }
    #endregion Setup

    public void PrepSpell(string launcherName, SpellComposition newComposition)
    {
        
        if (_launcherList[launcherName] != null)
        {
            var existingComposition = _launcherList[launcherName];
            //TODO here we might want to handle modifying existing composition
            
            
        }
        else
        {
            _launcherList[launcherName] = newComposition;
        }
    }
    
    //TODO Powen: work toward deprecating this
    public GameObject GetSpell(Enum_SpellShapes shape, Enum_Elements element, Vector3 newPosition, Quaternion newRotation = default)
    {
        if (_spellsList.ContainsKey(shape))
        {
            var pf = _spellsList[shape];
            var go = Instantiate(pf, newPosition, newRotation);
//            Dev.Log("go name:  " + go.name);
            var projectile = go.GetComponent<SpellBase>();
            projectile.ChangeElement(element);
            
            EVENT_NewSpellCast?.Invoke();
            
            return go;            
        }

        Dev.LogWarning("Cannot find spell of shape " + shape.ToString());
        return null;
    }

    
    //Powen: Might want to rename to get formed spell. Might want to return spellbase instead
    //SpellComposition is like a draft of spell.
    //Spell Components make up a Spell Composition.
    //This method will handle converting SpellComposition to actual GameObject/Spell that can damage things.
    public GameObject GetSpell(SpellComposition composition, Vector3 newPosition, Quaternion newRotation = default)
    {
        
        //var go =  this.GetSpell(composition.GetShape(), composition.GetElement(), newPosition, newRotation);

        var shape = composition.GetShape();
        var element = composition.GetElement();
        if (_spellsList.ContainsKey(shape))
        {
            var pf = _spellsList[shape];
            var go = Instantiate(pf, newPosition, newRotation);
//            Dev.Log("go name:  " + go.name);
            var projectile = go.GetComponent<SpellBase>();
            projectile.ChangeElement(element);
            projectile.SetupComposition(composition);
            
            EVENT_NewSpellCast?.Invoke();
            
            return go;            
        }

        Dev.LogWarning("Cannot find spell of shape " + shape.ToString());
        return null;
    }
    
    #region combo prep spell

    private bool IsShapeReady = false;
    private bool IsElementReady = false;
    private Enum_SpellShapes _preparedShape;
    private Enum_Elements _preparedElement;

    public void PrepSpell(Enum_SpellShapes shape)
    {
        Dev.Log("Prepared Shape: " + shape.ToString());
        _preparedShape = shape;
        _currentComposition.SetShape(shape);
        IsShapeReady = true;
        
        EVENT_NewSpellPreparation?.Invoke();
        
        if (IsSpellPrepped())
        {
            EVENT_NewSpellCast?.Invoke();
        }
    }

    public void PrepSpell(Enum_Elements element)
    {
        Dev.Log("Prepared Element: " + element.ToString());
        _preparedElement = element;
        _currentComposition.SetElement(element);
        IsElementReady = true;
        
        EVENT_NewSpellPreparation?.Invoke();
        
        if (IsSpellPrepped())
        {
            EVENT_NewSpellCast?.Invoke();
        }
    }

    public GameObject GetPrepSpell(Vector3 spellStartPosition, Quaternion startRotation = default)
    {
//        Dev.Log("Get Prep Spell");
        var spell = GetSpell(_preparedShape, _preparedElement, spellStartPosition, startRotation);
        // if (!spell)
        // {
        //     Dev.Log("Cannot get prep spell");
        // }
        IsShapeReady = false;
        IsElementReady = false;
        _preparedShape = Enum_SpellShapes.Sphere;
        _preparedElement = Enum_Elements.GrayNormal;

        EVENT_NewSpellCast?.Invoke();
        return spell;
    }
    
    public GameObject GetStoredSpell(Vector3 spellStartPosition, Quaternion startRotation = default)
    {
        if (_currentComposition.IsPrepped())
        {
            return GetSpell(_currentComposition.GetShape(), _currentComposition.GetElement(), spellStartPosition, startRotation);
        }
        return null;
    }

    public bool IsSpellPrepped()
    {
        return IsShapeReady && IsElementReady;
    }

    #endregion combo prep spell

    public bool IsElementPrepared()
    {
        return IsElementReady;
    }
    public bool IsShapePrepared()
    {
        return IsShapeReady;
    }

    public Sprite GetPrepElementSprite()
    {
        if (elementSpriteDictionary.ContainsKey(_preparedElement))
        {
            return elementSpriteDictionary[_preparedElement];    
        }

        Dev.LogWarning("No sprite in dictionary for element " + _preparedElement);
        return elementSpriteDictionary[Enum_Elements.GrayNormal];
    }

    public Sprite GetPrepShapeSprite()
    {
        if (shapeSpriteDictionary.ContainsKey(_preparedShape))
        {
            return shapeSpriteDictionary[_preparedShape];    
        }
        
        Dev.LogWarning("No sprite in dictionary for element " + _preparedShape);
        return shapeSpriteDictionary[Enum_SpellShapes.Sphere];
    }

}