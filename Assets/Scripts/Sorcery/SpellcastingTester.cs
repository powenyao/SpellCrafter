using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class SpellcastingTester : MonoBehaviour
{
    [SerializeField]
    private GameObject spell_1;

    [SerializeField]
    private GameObject spell_2;

    [SerializeField]
    //private Enum_XrController_Type _type = Enum_XrController_Type.Direct;
    //private Enum_XrController_Direction _direction = Enum_XrController_Direction.Left;
    // Start is called before the first frame update
    void Start()
    {
        DebugStart();
    }

    void DebugStart()
    {
        Core.Ins.Debug.AddDebugCodeForKeyboard(this.gameObject, nameof(SpellcastingTester), nameof(PlaceSpell),
            KeyCode.G, () => { PlaceSpell(spell_1); });
        Core.Ins.Debug.AddDebugCodeForKeyboard(this.gameObject, nameof(SpellcastingTester), nameof(PlaceSpell),
            KeyCode.H, () => { PlaceSpell(spell_2); });
    }

    // Update is called once per frame
    void Update()
    {
    }

    void PlaceSpell(GameObject go)
    {
        //Core.Ins.XRManager.ForceDropGrabbedItem(_direction, _type);
        //Core.Ins.XRManager.ForceSetGrabbedItem(Enum_XrController_Direction.Left, Enum_XrController_Type.Direct, go);
        //Core.Ins.XRManager.ForceSetGrabbedItem(XRNode.LeftHand, go);
    }
}