using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI spellStatsText;

    [SerializeField]
    TextMeshProUGUI enemyCountText;

    [SerializeField]
    TextMeshProUGUI narrationText;

    // Start is called before the first frame update
    void Start()
    {
        SetSpellStats(0, 0);
        SetEnemyCount(0);
        SetNarration("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpellStats(int count, float cost)
    {
        spellStatsText.text = $"Spells cast: {count}, Mana spent: {cost.ToString()}";
    }

    public void SetEnemyCount(int value)
    {
        enemyCountText.text = $"Targets left: {value}";
    }

    public void SetNarration(string text)
    {
        narrationText.text = text;
    }
}
