using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellComponentReference
{
    public static float Widen_ScaleMultiplier = 2f;
    public static float Concentrate_DamageMultiplier = 2f;
    public static float Speedup_Multiplier = 2f;
    public static float Speeddown_Multiplier = 0.5f;

    public static float GetElementalDamageMultiplier(Enum_Elements dealerElement, Enum_Elements receiveElement)
    {
        //Alt version
        if (dealerElement == Enum_Elements.GrayNormal)
        {
            return 1;
        }
        else if (receiveElement == dealerElement)
        {
            return 2;
        }
        else
        {
            return 0.5f;
        }

        //Simple version
        // if (receiveElement == dealerElement && dealerElement != Enum_Elements.GrayNormal)
        // {
        //     //Dev.Log("Elemental Multiplier of 3");
        //     return 3;
        // }
        // else
        // {
        //     //Dev.Log("Elemental Multiplier of 1");
        //     return 1;
        // }
    }
}
