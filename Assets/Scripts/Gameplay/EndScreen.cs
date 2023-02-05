using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.Gameplay;

public class EndScreen : MonoBehaviour
{
    public TMP_Text LastScoreText;

    public TMP_Text BestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        SetScoreString(ref LastScoreText, PlayerScores.LAST_SCORE_KEY, "Mana Spent");
        SetScoreString(ref BestScoreText, PlayerScores.BEST_SCORE_KEY, "Best");
    }

    private void SetScoreString(ref TMP_Text text, string scoreKey, string scoreLabel)
    {
        if (text != null && !string.IsNullOrEmpty(scoreKey) && !string.IsNullOrEmpty(scoreLabel))
        {
            Score score = PlayerScores.Get(scoreKey);
            if (score != null)
            {
                text.text = scoreLabel + ": " + score.manaCost.ToString();
            }
        }
    }
}
