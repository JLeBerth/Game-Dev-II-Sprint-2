using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class will be used to display lives
/// </summary>
public class Lives : MonoBehaviour
{
    public Image[] healthImages;
    public Sprite[] healthSprites;

    // Initialize variables here
    private void Start()
    {
        if (GameStats.currentState == GameState.Title)
        {
            GameStats.curHealth = GameStats.startHearts * GameStats.healthPerHeart;
        }

        GameStats.maxHealth = GameStats.maxHeartAmount * GameStats.healthPerHeart;
        checkHealthAmount();
    }
    
    // Update
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            Heal(1);
        }
    }

    /// <summary>
    /// Method to check health and update the sprites
    /// </summary>
    void checkHealthAmount()
    {
        for (int i = 0; i < GameStats.maxHeartAmount; i++)
        {
            if(GameStats.startHearts <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }

        UpdateHearts();
    }

    /// <summary>
    /// Method to update drawing the hearts
    /// </summary>
    void UpdateHearts()
    {
        bool empty = false;
        int i = 0;

        foreach(Image image in healthImages)
        {
            if(empty)
            {
                image.sprite = healthSprites[0];
            }
            else
            {
                i++;
                if(GameStats.curHealth >= i * GameStats.healthPerHeart)
                {
                    image.sprite = healthSprites[healthSprites.Length - 1];
                }
                else
                {
                    int currentHeartHealth = (int)(GameStats.healthPerHeart - (GameStats.healthPerHeart * i - GameStats.curHealth));
                    int healthPerImage = GameStats.healthPerHeart / (healthSprites.Length - 1);
                    int imageIndex = currentHeartHealth / healthPerImage;
                    image.sprite = healthSprites[imageIndex];
                    empty = true;
                }
            }
        }
    }

    /// <summary>
    /// Method for player taking damage
    /// </summary>
    public void TakeDamage(int amount)
    {
        if(GameStats.curHealth > 0)
        {
            GameStats.curHealth -= amount;
            GameStats.curHealth = Mathf.Clamp(GameStats.curHealth, 0, GameStats.startHearts * GameStats.healthPerHeart);
            UpdateHearts();
        }
    }

    /// <summary>
    /// Method for player healing damage
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        if (GameStats.curHealth != GameStats.startHearts)
        {
            GameStats.curHealth += amount;
            GameStats.curHealth = Mathf.Clamp(GameStats.curHealth, 0, GameStats.startHearts * GameStats.healthPerHeart);
            UpdateHearts();
        }
    }

    /// <summary>
    /// Adding hearts
    /// Restores health when you collect a heart
    /// </summary>
    public void AddHeartContainer()
    {
        GameStats.startHearts++;
        GameStats.startHearts = Mathf.Clamp(GameStats.startHearts, 0, GameStats.maxHeartAmount);

        GameStats.curHealth = GameStats.startHearts * GameStats.healthPerHeart;
        GameStats.maxHealth = GameStats.maxHeartAmount * GameStats.healthPerHeart;

        checkHealthAmount();
    }
}
