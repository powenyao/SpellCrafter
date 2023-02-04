﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Source data for UI panels and choices.
/// 
/// TODO: This is a placeholder approach, can add colors or link to existing enums, etc.
/// </summary>
static class CraftingAspects
{
    static readonly Dictionary<string, string[]> OptionsByName = new Dictionary<string, string[]>
    {
        {
            "Elements",
            //new string[] { "Fire", "Water", "Wind", "Electric"}
            new string[] {
                Enum_Elements.GrayNormal.ToString(),
                Enum_Elements.OrangePyro.ToString(),
                Enum_Elements.GreenDendro.ToString(),
                Enum_Elements.YellowGeo.ToString(),
                Enum_Elements.TealAnemo.ToString(), 
                Enum_Elements.PurpleElectro.ToString(),
                Enum_Elements.BlueHydro.ToString()}
        },
        {
            "Forms",
            //new string[] { "Ball", "Rod"}
            new string[] { Enum_SpellShapes.Sphere.ToString(), Enum_SpellShapes.Cylinder.ToString() }
        },
        {
            "Modifiers",
            //new string[] { "Concentrate", "Widen", "Pull",}
            new string[] { Enum_SpellComponents_Effects.Concentrate.ToString(), Enum_SpellComponents_Effects.Widen.ToString(), Enum_SpellComponents_Effects.Pull.ToString() }
        }
    };

    public static string[] GetOptions(string name)
    {
        return OptionsByName[name];
    }
}
