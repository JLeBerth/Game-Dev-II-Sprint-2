using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Update the current game state
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((int)GameStats.currentState != sceneIndex)
        {
            GameStats.currentState = (GameState)sceneIndex;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current State is " + GameStats.currentState.ToString());
        switch (GameStats.currentState)
        {
            case GameState.Title:
                // Title -> Introduction: Press Enter key
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameStats.currentState = GameState.Introduction;
                    SceneManager.LoadScene("Introduction");
                }
                break;
            case GameState.Introduction:
                ReachCheckPoint("SnowScene");
                break;
            case GameState.SnowScene:
                ReachCheckPoint("WaterScene");
                break;
            case GameState.WaterScene:
                break;
            case GameState.Pause:
                break;
            case GameState.Lose:
                break;
            case GameState.Win:
                break;
        }
    }

    // Checks if the player reaches the check point, if true, then load the next scene
    void ReachCheckPoint(string sceneName)
    {
        GameObject checkpoint = GameObject.Find("Checkpoint");
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (checkpoint != null && Mathf.Abs(playerPos.x - checkpoint.transform.position.x) < 2f)
        {
            GameStats.currentState = GameState.SnowScene;
            SceneManager.LoadScene(sceneName);
        }
    }
}
