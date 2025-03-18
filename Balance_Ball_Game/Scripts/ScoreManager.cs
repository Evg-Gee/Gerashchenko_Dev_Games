using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreManager : IScoreManager
{
    private int score;
    public event Action<int> OnScoreChanged = delegate { };
    private readonly Dictionary<Color, int> scoreMap = new Dictionary<Color, int>
    {
        { Color.red, 10 },
        { Color.magenta, 20 },
        { Color.cyan, 30 }
    };
    

    public void AddScore(Color color)
    {
        if (scoreMap.TryGetValue(color, out int value))
        {
            score += value;
        }
    }
    public void SubtractScore(Color color)
    {
        if (scoreMap.TryGetValue(color, out int value))
        {
            score -= value;
            OnScoreChanged?.Invoke(score);
        }
    }
    

    public int GetCurrentScore() => score;
}