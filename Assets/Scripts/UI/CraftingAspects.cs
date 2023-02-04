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
static class CraftingAspects
{
    static readonly Dictionary<string, string[]> OptionsByName = new Dictionary<string, string[]>
    {
        {
            "Elements",
            new string[] { "Fire", "Water", "Wind", "Electric"}
        },
        {
            "Forms",
            new string[] { "Ball", "Rod" }
        },
        {
            "Modifiers",
            new string[] { "Concentrate", "Widen", "Pull" }
        }
    };

    public static string[] GetOptions(string name)
    {
        return OptionsByName[name];
    }
}
