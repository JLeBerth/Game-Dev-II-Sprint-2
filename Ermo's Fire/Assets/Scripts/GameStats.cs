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
    Win,
    Pause
}

public class GameStats : MonoBehaviour
{
    public static GameState currentState = GameState.Title;

}
