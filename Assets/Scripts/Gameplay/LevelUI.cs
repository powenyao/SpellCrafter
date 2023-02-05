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
        spellStatsText.text = $"Cast {count} spells with cost {cost.ToString()}";
    }

    public void SetEnemyCount(int value)
    {
        enemyCountText.text = $"{value} enemies left";
    }

    public void SetNarration(string text)
    {
        narrationText.text = text;
    }
}
