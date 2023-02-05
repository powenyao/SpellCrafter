using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public void Open()
    {
        Application.OpenURL("https://github.com/powenyao/SpellCrafter");
    }
}
