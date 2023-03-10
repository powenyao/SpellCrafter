using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.Gameplay
{
    [Serializable]
    public class Score
    {
        public int numSpells = int.MaxValue;
        public float manaCost = float.MaxValue;
    }

    public static class PlayerScores
    {
        public static readonly string LAST_SCORE_KEY = "LastScore";
        public static readonly string BEST_SCORE_KEY = "BestScore";

        public static void InitializeBestScore()
        {
            Score best = Get(BEST_SCORE_KEY);
            if (best.numSpells == 0 && best.manaCost == 0)
            {
                Set(BEST_SCORE_KEY, new Score() { numSpells = int.MaxValue, manaCost = int.MaxValue });
            }
        }

        public static Score Get(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return new Score();
            }

            string data = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<Score>(data);
        }

        static void Set(string key, Score score)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(score));
        }

        public static void SetNewScore(int numSpells, float manaCost)
        {
            Score cur = new Score() { numSpells = numSpells, manaCost = manaCost };
            Set(LAST_SCORE_KEY, cur);

            // Anytime a new score is added, it should be compared against the best score:
            UpdateBest(cur);
        }

        private static bool UpdateBest(Score score)
        {
            Score best = Get(BEST_SCORE_KEY);
            if (score.manaCost < best.manaCost)
            {
                Set(BEST_SCORE_KEY, score);
                return true;
            }

            return false;
        }
    }
}
