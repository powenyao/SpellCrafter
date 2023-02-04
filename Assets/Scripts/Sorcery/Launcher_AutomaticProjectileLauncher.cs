using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher_AutomaticProjectileLauncher : LauncherBase
{
    [SerializeField]
    private string _castPopupText = "Default Cast Firebolt";

    [SerializeField]
    private Enum_SpellShapes shape = Enum_SpellShapes.Sphere;

    [SerializeField]
    private Enum_Elements element = Enum_Elements.OrangePyro;

    //How long it takes to launch another spell
    [SerializeField]
    private float launchCooldown = 1f;

    //internal for tracking time passed since casting
    private float timePassed = 0;

    [SerializeField]
    private List<string> spellComponentEffectsString = new List<string>();

    protected override void Setup()
    {
        base.Setup();

        ComposeSpell();
    }

    protected override void Process()
    {
        if (timePassed >= launchCooldown)
        {
            Launch();
            timePassed = 0;
        }
        else
        {
            timePassed += Time.deltaTime;
        }
    }

    protected override void Launch()
    {
        base.Launch();
        Core.Ins.UIEffectsManager.RequestPopUp(this.transform, _castPopupText);
    }

    [ContextMenu("ComposeSpell")]
    protected override void ComposeSpell()
    {
        composition = new SpellComposition(shape, element);
    }

    #region Debug Tools for Compose Spell

    [ContextMenu("ComposeSpellWithConcentrate")]
    void DebugComposeSpellWithConcentrate()
    {
        composition = new SpellComposition(shape, element);
        composition.AddSpellComponent(Enum_SpellComponentCategories.Effects,
            Enum_SpellComponents_Effects.Concentrate.ToString());
    }

    [ContextMenu("ComposeSpellWithWiden")]
    void DebugComposeSpellWithWiden()
    {
        composition = new SpellComposition(shape, element);
        composition.AddSpellComponent(Enum_SpellComponentCategories.Effects,
            Enum_SpellComponents_Effects.Widen.ToString());
    }

    [ContextMenu("ComposeSpellWithStringList")]
    void DebugComposeSpellWithStringList()
    {
        composition = new SpellComposition(shape, element);
        foreach (var s in spellComponentEffectsString)
        {
            composition.AddSpellComponent(Enum_SpellComponentCategories.Effects, s);
        }
    }

    #endregion Debug Tools for Compose Spell
}