using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI spellCountText;

    // Start is called before the first frame update
    void Start()
    {
        SetSpellCount(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpellCount(int value)
    {
        spellCountText.text = "Spells cast: " + value.ToString();
    }
}
