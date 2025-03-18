using UnityEngine;

public interface IScoreManager
{
    void AddScore(Color color);
    int GetCurrentScore();
}