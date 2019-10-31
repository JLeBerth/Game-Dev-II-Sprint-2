using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class will be used to display lives
/// </summary>
public class Lives : MonoBehaviour
{
    private int maxHeartAmount = 3;
    public int startHearts = 3;
    public int curHealth;
    private int maxHealth;
    private int healthPerHeart = 1;

    public Image[] healthImages;
    public Sprite[] healthSprites;

    // Initialize variables here
    private void Start()
    {
        curHealth = startHearts * healthPerHeart;
        maxHealth = maxHeartAmount * healthPerHeart;
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
        for (int i = 0; i < maxHeartAmount; i++)
        {
            if(startHearts <= i)
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
                if(curHealth >= i * healthPerHeart)
                {
                    image.sprite = healthSprites[healthSprites.Length - 1];
                }
                else
                {
                    int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart * i - curHealth));
                    int healthPerImage = healthPerHeart / (healthSprites.Length - 1);
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
        if(curHealth > 0)
        {
            curHealth -= amount;
            curHealth = Mathf.Clamp(curHealth, 0, startHearts * healthPerHeart);
            UpdateHearts();
        }
    }

    /// <summary>
    /// Method for player healing damage
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        if (curHealth != startHearts)
        {
            curHealth += amount;
            curHealth = Mathf.Clamp(curHealth, 0, startHearts * healthPerHeart);
            UpdateHearts();
        }
    }

    /// <summary>
    /// Adding hearts
    /// Restores health when you collect a heart
    /// </summary>
    public void AddHeartContainer()
    {
        startHearts++;
        startHearts = Mathf.Clamp(startHearts, 0, maxHeartAmount);

        curHealth = startHearts * healthPerHeart;
        maxHealth = maxHeartAmount * healthPerHeart;

        checkHealthAmount();
    }
}
