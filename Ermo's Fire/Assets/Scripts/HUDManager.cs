using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    /// <summary>
    /// HUDManager is a player-attached script that functions as a place to handle the logic for displaying 
    /// - active rune selection
    /// - radial ui logic to know where to place icons
    /// - maybe any other future UI considerations as well.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        // Pointer to the player character, used to update rune selection / active rune flag
        Platformer2DUserControl m_Character;

        public GameObject uiSprites;

        // How fast to linearly interpolate for the rune icons
        public float lerpSpeed = 25.0f;

        // How far from player to display rune icons
        public float runeRadius = 4.0f;

        private void Awake()
        {
            // Grab ref to player character
            m_Character = GetComponent<Platformer2DUserControl>();
        }

        void Start()
        {
        }

        void Update()
        {
            // If flag enabled: 
            // - set runes to visible
            // - TODO: lerp from player to outer radius position
            if (m_Character.m_Character.m_Selection)
            {
                int nRunes = uiSprites.transform.childCount;

                for(int i = 0; i < nRunes; ++i)
                {
                    float xPos = gameObject.transform.position.x + runeRadius * Mathf.Cos(Mathf.PI * (i + 1) / (nRunes + 1));
                    float yPos = gameObject.transform.position.y + runeRadius * Mathf.Sin(Mathf.PI * (i + 1) / (nRunes + 1));
                    Vector2 neededPosition = new Vector2(xPos, yPos);
                    uiSprites.transform.GetChild(i).position = Vector2.Lerp(uiSprites.transform.GetChild(i).position, neededPosition, lerpSpeed * Time.deltaTime);
                }
            }
            else
            {
                ResetRunes();
            }

        }

        /// <summary>
        ///  Lerp back to the player's center
        /// </summary>
        private void ResetRunes()
        {
            for(int i = 0; i < uiSprites.transform.childCount; ++i)
            {
                uiSprites.transform.GetChild(i).position = Vector2.Lerp(uiSprites.transform.GetChild(i).position, this.gameObject.transform.position, lerpSpeed * Time.deltaTime);
            }
        }

    }
}
