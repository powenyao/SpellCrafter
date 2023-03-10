using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher_Player : LauncherBase
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    CraftingMenu craftingMenu;

    protected override void Setup()
    {
        base.Setup();
        _launcherIdentifier = "Player";
        sorcery.PrepSpellWithComposition(_launcherIdentifier, new SpellComposition(), true);

        //_lineRenderer.SetPosition(0, launchTransform.position);
        //_lineRenderer.SetPosition(1, launchTransform.position + launchTransform.forward*50);
        if (!_lineRenderer)
        {
            Dev.LogError("[Launcher_Player] Setup > missing line renderer");
        }

        craftingMenu.OnSpellPrepped += newComposition =>
        {
            var newColor = Core.Ins.UIEffectsManager.GetColorForElement(newComposition.GetElement());
            //_lineRenderer.startColor = newColor;
            //_lineRenderer.endColor = newColor;
            _lineRenderer.material.color = newColor;
        };
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

    private float lastLaunchTime = 0f;

    [ContextMenu("Launch")]
    protected override void Launch()
    {
        if (isCoolingdown)
        {
            var remainingTime = lastLaunchTime + SpellComponentReference.PlayerLauncher_Cooldown - Time.time;
//            Debug.Log("[Launcher_Player] Launch > cooldown needs " + remainingTime + " seconds");
            return;
        }

        var projectile = sorcery.GetSpell(_launcherIdentifier, launchTransform.position,
            launchTransform.rotation);
        if (projectile)
        {
            var spell = projectile.GetComponent<SpellBase>();
            spell.Cast();
            lastLaunchTime = Time.time;
        }
        else
        {
            Dev.LogWarning("Spell was not ready");
        }
    }

    [ContextMenu("Add Widen")]
    protected void DebugAddWiden()
    {
        sorcery.PrepSpell(_launcherIdentifier, Enum_SpellComponentCategories.Effects,
            Enum_SpellComponents_Effects.Widen.ToString());
    }

    [ContextMenu("Add SpeedUp")]
    protected void DebugAddSpeedUp()
    {
        sorcery.PrepSpell(_launcherIdentifier, Enum_SpellComponentCategories.Effects,
            Enum_SpellComponents_Effects.SpeedUp.ToString());
    }

    [ContextMenu("Add SpeedDown")]
    protected void DebugAddSpeedDown()
    {
        sorcery.PrepSpell(_launcherIdentifier, Enum_SpellComponentCategories.Effects,
            Enum_SpellComponents_Effects.SpeedDown.ToString());
    }

    RaycastHit Hit;
    private bool hitBlocked = false;
    float maxLaserDistance = 50f;
    private bool isCoolingdown = false;

    void LateUpdate()
    {
        isCoolingdown = Time.time < lastLaunchTime + SpellComponentReference.PlayerLauncher_Cooldown;
        //DebugUpdate();

        //if(Physics.Raycast(transform.position, transform.forward, out Hit, maxLaserDistance, ~ignoredLayers)){
        if (Physics.Raycast(launchTransform.position, launchTransform.forward, out Hit, maxLaserDistance))
        {
            hitBlocked = true;
            // do stuff
        }
        else
        {
            hitBlocked = false;
        }
        
        if (_lineRenderer)
        {
            _lineRenderer.enabled = !isCoolingdown;

            _lineRenderer.SetPosition(0, launchTransform.position);
            
            if (hitBlocked) // we've hit something, so our line renderer end point should stop here
            {
                _lineRenderer.SetPosition(1, Hit.point);
            }
            else
            {
                _lineRenderer.SetPosition(1, launchTransform.position + launchTransform.forward * maxLaserDistance);
            }
        }
    }

    void DebugUpdate()
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