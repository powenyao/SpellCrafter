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
        //SetupDebug();
    }

    void SetupSpells()
    {
        //Setup Spells
        _spellsList.Add(Enum_SpellShapes.Sphere, PF_Basic);
        _spellsList.Add(Enum_SpellShapes.Horizontal, PF_Horizontal);
        _spellsList.Add(Enum_SpellShapes.Cross, PF_Cross);
        _spellsList.Add(Enum_SpellShapes.Wall, PF_Wall);
        _spellsList.Add(Enum_SpellShapes.Spikes, PF_Spikes);
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
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Null",
            () => this.PrepSpell("Player", Enum_Elements.GrayNormal));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Fire",
            () => this.PrepSpell("Player", Enum_Elements.OrangePyro));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Wood",
            () => this.PrepSpell("Player", Enum_Elements.GreenDendro));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Earth",
            () => this.PrepSpell("Player", Enum_Elements.YellowGeo));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Wind",
            () => this.PrepSpell("Player", Enum_Elements.TealAnemo));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Electric",
            () => this.PrepSpell("Player", Enum_Elements.PurpleElectro));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepElement Water",
            () => this.PrepSpell("Player", Enum_Elements.BlueHydro));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Sphere",
            () => this.PrepSpell("Player", Enum_SpellShapes.Sphere));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Wall",
            () => this.PrepSpell("Player", Enum_SpellShapes.Wall));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Cross",
            () => this.PrepSpell("Player", Enum_SpellShapes.Cross));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Bar",
            () => this.PrepSpell("Player", Enum_SpellShapes.Horizontal));
        Core.Ins.Debug.AddDebugCode(this.gameObject, nameof(Subservice_Sorcery), "PrepShape Spikes",
            () => this.PrepSpell("Player", Enum_SpellShapes.Spikes));
    }

    #endregion Setup

    //This is the section where someone gets a fully formed spell. At the minimum the requester need to provide shape and element. 
    //I think position and rotation is provided so that the object don't spawn at a location then quickly move to where its supposed to be. 
    //We had this issue before

    #region GetSpell

    //TODO Powen: work toward deprecating this
    public GameObject GetSpell(Enum_SpellShapes shape, Enum_Elements element, Vector3 newPosition,
        Quaternion newRotation = default)
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

    public GameObject GetSpell(string identifier, Vector3 newPosition, Quaternion newRotation = default)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            if (composition.IsPrepped())
            {
                var spell = GetSpell(composition, newPosition, newRotation);

                // Would like last selection to continue
                // composition.Clear();

                EVENT_NewSpellCast?.Invoke();
                return spell;
            }
        }

        Dev.Log("[Subservice_Sorcery.cs] GetSpell > no composition");
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

        Dev.LogWarning("[Subservice_Sorcery.cs] GetSpell > Cannot find spell of shape " + shape.ToString());
        return null;
    }

    #endregion GetSpell

    #region prep spell

    //Todo should this be renamed as WithNewComposition and only deal with that?
    public void PrepSpellWithComposition(string identifier, SpellComposition newComposition, bool replaceIfExists = false)
    {
        if (_launcherList.ContainsKey(identifier) && !replaceIfExists)
        {
            var existingComposition = _launcherList[identifier];
            
            //TODO here we might want to handle modifying existing composition
            existingComposition.MergeComposition(newComposition);
        }
        else
        {
            _launcherList[identifier] = newComposition;
        }
    }

    //private bool IsShapeReady = false;
    //private bool IsElementReady = false;
    //private Enum_SpellShapes _preparedShape;
    //private Enum_Elements _preparedElement;

    public void PrepSpell(string identifier, Enum_SpellShapes shape)
    {
        if (_launcherList[identifier] != null)
        {
            Dev.Log("Prepared Shape: " + shape.ToString());

            _launcherList[identifier].SetShape(shape);
            //_preparedShape = shape;
            //_currentComposition.SetShape(shape);
            //IsShapeReady = true;

            EVENT_NewSpellPreparation?.Invoke();

            if (IsSpellPrepped(identifier))
            {
                EVENT_NewSpellCast?.Invoke();
            }
        }
    }

    public void PrepSpell(string identifier, Enum_SpellComponentCategories categories, string value)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            composition.AddSpellComponent(categories, value);

            _launcherList[identifier] = composition;
        }
    }

    public void PrepSpell(string identifier, Enum_Elements element)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            Dev.Log("Prepared Element: " + element.ToString());
            //_preparedElement = element;
            composition.SetElement(element);

            EVENT_NewSpellPreparation?.Invoke();

            if (composition.IsPrepped())
            {
                EVENT_NewSpellCast?.Invoke();
            }
        }
    }


    /// <summary>
    /// Helper method to get the spell composition used by one of the launcher
    /// </summary>
    /// <param name="identifier">string identifier of a launcher</param>
    /// <returns></returns>
    private SpellComposition GetCompositionOfLauncher(string identifier)
    {
        if (_launcherList.ContainsKey(identifier))
        {
            return _launcherList[identifier];
        }

        return null;
    }

    #endregion combo prep spell

    #region Spell Status Getter

    public bool IsSpellPrepped(string identifier)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            return composition.IsPrepped();
            //return IsShapeReady && IsElementReady;
        }

        return false;
    }

    public bool IsElementPrepared(string identifier)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            return composition.IsElementReady();
            //return IsElementReady;
        }

        return false;
    }

    public bool IsShapePrepared(string identifier)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            return composition.IsShapeReady();
            //return IsShapeReady;
        }

        return false;
    }

    #endregion Spell Status Getter

    #region Sprite

    public Sprite GetPrepElementSprite(string identifier)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            var _preparedElement = composition.GetElement();
            if (elementSpriteDictionary.ContainsKey(_preparedElement))
            {
                return elementSpriteDictionary[_preparedElement];
            }

            Dev.LogWarning("[Subservice_Sorcery.cs] GetPrepElementSprite >No sprite in dictionary for element " +
                           _preparedElement);
            return elementSpriteDictionary[Enum_Elements.GrayNormal];
        }

        Dev.LogWarning("[Subservice_Sorcery.cs] GetPrepElementSprite > No spell composition for launcher " +
                       identifier);
        return elementSpriteDictionary[Enum_Elements.GrayNormal];
    }

    public Sprite GetPrepShapeSprite(string identifier)
    {
        var composition = GetCompositionOfLauncher(identifier);
        if (composition != null)
        {
            var _preparedShape = composition.GetShape();
            if (shapeSpriteDictionary.ContainsKey(_preparedShape))
            {
                return shapeSpriteDictionary[_preparedShape];
            }

            Dev.LogWarning("[Subservice_Sorcery.cs] GetPrepShapeSprite > No sprite in dictionary for element " +
                           _preparedShape);
            return shapeSpriteDictionary[Enum_SpellShapes.Sphere];
        }

        Dev.LogWarning("[Subservice_Sorcery.cs] GetPrepShapeSprite > No spell composition for launcher " + identifier);
        return shapeSpriteDictionary[Enum_SpellShapes.Sphere];
    }

    #endregion Sprite
}