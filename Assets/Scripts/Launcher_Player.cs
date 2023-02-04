using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher_Player : LauncherBase
{
    protected override void Setup()
    {
        base.Setup();
        _launcherIdentifier = "Player";
        sorcery.PrepSpellWithComposition(_launcherIdentifier, new SpellComposition(), true);
    }
    
    [ContextMenu("Compose Spell With Widen")]
    protected virtual void DebugGetComposedSpellWithWiden()
    {
        this.ComposeSpell();
        composition.AddSpellComponent(Enum_SpellComponentCategories.Effects, "Widen");
        //composition = new SpellComposition(Enum_SpellShapes.Sphere, Enum_Elements.GrayNormal);
        sorcery.PrepSpellWithComposition(_launcherIdentifier, composition);
    }

    [ContextMenu("Compose Spell With SpeedUp")]
    protected virtual void ComposeSpellWithSpeedUp()
    {
        composition.AddSpellComponent(Enum_SpellComponentCategories.Effects, "SpeedUp");
        sorcery.PrepSpellWithComposition(_launcherIdentifier, composition);
    }
    
    [ContextMenu("Launch")]
    protected override void Launch()
    {
        var projectile = sorcery.GetSpell(_launcherIdentifier, launchTransform.position,
            launchTransform.rotation);
        if (projectile)
        {
            var spell = projectile.GetComponent<SpellBase>();
            spell.Cast();    
        }
        else
        {
            Dev.LogWarning("Spell was not ready");
        }
    }

    [ContextMenu("Add Widen")]
    protected void DebugAddWiden()
    {
        sorcery.PrepSpell(_launcherIdentifier, Enum_SpellComponentCategories.Effects, Enum_SpellComponents_Effects.Widen.ToString());
    }
    
    [ContextMenu("Add SpeedUp")]
    protected void DebugAddSpeedUp()
    {
        sorcery.PrepSpell(_launcherIdentifier, Enum_SpellComponentCategories.Effects, Enum_SpellComponents_Effects.SpeedUp.ToString());
    }

    void Update()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>(); //Setup
        
        if (kb.digit1Key.wasReleasedThisFrame)
        {
            //Dev.Log("1 Key");
            DebugAddWiden();
        }
        if (kb.digit2Key.wasReleasedThisFrame)
        {
            //Dev.Log("2 Key");
            DebugAddSpeedUp();
        }
    }
}
