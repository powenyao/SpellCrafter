using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


//https://docs.google.com/spreadsheets/d/1ecp5bPLOy_fXQO3aUuUc46Q6hXONMFKgxgODaQ4zC2k/edit#gid=1150781586
//Interaction Mode
//Single
//Repeatable
//Charge

public class LauncherBase : MonoBehaviour
{
    [SerializeField]
    protected string _launcherIdentifier = "Default Auto LauncherBase";

    //Where the projectile is launched from
    [SerializeField]
    protected Transform launchTransform;

    //Where the projectile is going toward
    [SerializeField]
    private Transform targetTransform;

    protected Subservice_Sorcery sorcery;
    protected SpellComposition composition;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        if (!launchTransform)
        {
            launchTransform = transform;
        }

        if (!targetTransform)
        {
            targetTransform.position = launchTransform.position - launchTransform.forward * 1;
        }

        sorcery = (Subservice_Sorcery)Core.Ins.Subservices.GetSubservice(nameof(Subservice_Sorcery));
    }

    // Update is called once per frame
    void Update()
    {
        Process();
    }

    protected virtual void Process()
    {
    }

    [ContextMenu("Launch")]
    protected virtual void Launch()
    {
        var projectile = sorcery.GetSpell(composition, launchTransform.position,
            launchTransform.rotation);
        var spell = projectile.GetComponent<SpellBase>();
        spell.Cast();
    }

    [ContextMenu("ComposeSpell")]
    protected virtual void ComposeSpell()
    {
        composition = new SpellComposition(Enum_SpellShapes.Sphere, Enum_Elements.GrayNormal);
    }

    public virtual void TryLaunch()
    {

    }
}