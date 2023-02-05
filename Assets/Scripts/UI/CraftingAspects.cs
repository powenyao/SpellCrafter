using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Source data for UI panels and choices.
/// 
/// TODO: This is a placeholder approach, can add colors or link to existing enums, etc.
/// </summary>
public static class CraftingAspects
{
    public enum AspectName
    {
        Elements,
        Forms,
        Components,
        Tracking,
        Path,
        Trigger
    }

    static readonly Dictionary<AspectName, string[]> OptionsByName = new Dictionary<AspectName, string[]>
    {
        {
            AspectName.Elements,
            //new string[] { "Fire", "Water", "Wind", "Electric"}
            // new string[] {
            //     Enum_Elements.GrayNormal.ToString(),
            //     Enum_Elements.OrangePyro.ToString(),
            //     Enum_Elements.GreenDendro.ToString(),
            //     Enum_Elements.YellowGeo.ToString(),
            //     Enum_Elements.TealAnemo.ToString(),
            //     Enum_Elements.PurpleElectro.ToString(),
            //     Enum_Elements.BlueHydro.ToString()}
            Enum.GetNames(typeof(Enum_Elements))
        },
        {
            AspectName.Forms,
            //new string[] { "Ball", "Rod"}
            //new string[] { Enum_SpellShapes.Sphere.ToString(), Enum_SpellShapes.Cylinder.ToString() }
            Enum.GetNames(typeof(Enum_SpellShapes))
        },
        {
            AspectName.Components,
            //new string[] { "Concentrate", "Widen", "Pull",}
            //new string[] { Enum_SpellComponents_Effects.Concentrate.ToString(), Enum_SpellComponents_Effects.Widen.ToString(), Enum_SpellComponents_Effects.Pull.ToString() }
            Enum.GetNames(typeof(Enum_SpellComponents_Effects))
        },
        {
            AspectName.Tracking,
            Enum.GetNames(typeof(Enum_SpellComponents_Tracking))
        },
        {
            AspectName.Path,
            Enum.GetNames(typeof(Enum_SpellComponents_Path))
        }
    };

    public static string[] GetOptions(AspectName name)
    {
        return OptionsByName[name];
    }
}
