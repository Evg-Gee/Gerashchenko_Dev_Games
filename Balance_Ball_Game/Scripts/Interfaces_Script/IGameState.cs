using System;

public interface IGameState
{
    bool IsGameOver();
    event Action OnGameOver;
}