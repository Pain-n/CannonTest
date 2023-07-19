using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GlobalContext : MonoBehaviour
{
    public static GlobalContext Instance;

    [HideInInspector]
    public List<int> BestScores;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BestScores = new List<int>(5);
    }

    public void WriteScore(int score) 
    {
        if (score == 0 || BestScores.Contains(score)) return;

        int newScoreID = -1;
        for (int i = 0; i < BestScores.Count; i++)
        {
            if (BestScores[i] < score)
            {
                newScoreID = i;
                break;
            }
        }
        if (newScoreID == -1 && BestScores.Count == BestScores.Capacity) return;
        else if (newScoreID == -1)
        {
            BestScores.Add(score);
            return;
        }

        List<int> newScores = new List<int>(BestScores.Capacity);

        for(int i = 0; i < newScores.Capacity; i++)
        {
            if(i == newScoreID)
            {
                newScores.Add(score);
            }
            if (i < BestScores.Count && newScores.Count < newScores.Capacity) newScores.Add(BestScores[i]);
            else break;
        }
        BestScores = newScores;
    }

}
