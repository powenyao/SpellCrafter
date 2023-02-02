using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField]
    private string CastPopupText = "Cast Firebolt";
    [SerializeField]
    private Enum_SpellShapes shape = Enum_SpellShapes.Sphere;
    [SerializeField]
    private Enum_Elements element = Enum_Elements.OrangePyro;
    
    //Where the projectile is launched from
    [SerializeField]
    private Transform launchTransform;

    //Where the projectile is going toward
    [SerializeField]
    private Transform targetTransform;


    //How long it takes to launch another spell
    [SerializeField]
    private float launchCooldown = 1f;

    //internal for tracking time passed since casting
    private float timePassed = 0;

    private Subservice_Sorcery sorcery;

    // Start is called before the first frame update
    void Start()
    {
        if (!launchTransform)
        {
            launchTransform = transform;
        }

        targetTransform.position = launchTransform.position - launchTransform.forward * 1;
        sorcery = (Subservice_Sorcery)Core.Ins.Subservices.GetSubservice(nameof(Subservice_Sorcery));
    }

    // Update is called once per frame
    void Update()
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

    [ContextMenu("Launch")]
    void Launch()
    {
        Core.Ins.UIEffectsManager.RequestPopUp(this.transform, CastPopupText);

        //var projectile = GameObject.Instantiate(PF_Projectile);
        var projectile = sorcery.GetSpell(shape, element, launchTransform.position);
        
        //projectile.transform.position = launchTransform.position;
        var spell = projectile.GetComponent<SpellBase>();
        spell.Cast(targetTransform);
    }
}