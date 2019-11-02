using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Title,
    Introduction,
    SnowScene,
    WaterScene,
    Lose,
    Win
}

public static class GameStats
{
    public static GameState currentState = GameState.Title;

    public static int maxHeartAmount = 3;
    public static int startHearts = 3;
    public static int curHealth;
    public static int maxHealth;
    public static int healthPerHeart = 1;
}
