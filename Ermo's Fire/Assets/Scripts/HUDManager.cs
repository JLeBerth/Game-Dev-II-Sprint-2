using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HUDManager is a player-attached script that functions as a place to handle the logic for displaying 
/// - active rune selection
/// - radial ui logic to know where to place icons
/// - maybe any other future UI considerations as well.
/// </summary>
public class HUDManager : MonoBehaviour
{
    // Flag for whether we're selecting an active rune or not
    public bool RuneSelection = true;

    // How many runes and the list to hold their positions
    // TODO: runecount is const by design at the moment, 
    // for our goal of having a game with a flexible inventory (being able to set down or carry a rune w/ you), this must eventually be changed to allow for some sort of "abilityManager" to hold the active runes
    // reminder: Linked Lists!
    public const int runeCount = 3;
    public Vector2[] l_runePositions;

    // List of rune game objects
    public GameObject[] l_runeSprites;

    // How far from player to display rune icons
    public float runeRadius = 4.0f;

    private void Awake()
    {
        // Initialize rune icon positions
        l_runePositions = new Vector2[runeCount];

        // Initialize rune sprite array
        l_runeSprites = new GameObject[runeCount];

        UpdateRunePositions();
    }

    void Start()
    {
        // Get all child game objects into our list
        for(int i = 0; i < runeCount; ++i)
        {
            l_runeSprites[i] = this.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        UpdateRunePositions();

        // If flag enabled: 
        // - set runes to visible
        // - TODO: lerp from player to outer radius position
        if (RuneSelection)
        {
            for (int i = 0; i < runeCount; ++i)
            {
                l_runeSprites[i].transform.position = new Vector3(l_runePositions[i].x, l_runePositions[i].y, 0.0f);
            }
        }
        else
        {
            ResetRunes();
        }
    }

    private void UpdateRunePositions()
    {
        // For each rune, calculate its radial position with regards to the player's position
        for(int i = 0; i < runeCount; ++i)
        {
            float xPos = gameObject.transform.position.x + runeRadius * Mathf.Cos(Mathf.PI * (i+1) / (runeCount+1));
            float yPos = gameObject.transform.position.y + runeRadius * Mathf.Sin(Mathf.PI * (i+1) / (runeCount+1));
            Vector2 vec = new Vector2(xPos, yPos);
            l_runePositions[i] = vec;
        }
    }

    private void ResetRunes()
    {
        // For now, tp all runes back into the character

        foreach (GameObject rune in l_runeSprites)
        {
            rune.transform.position = this.gameObject.transform.position;
        }
    }

}
