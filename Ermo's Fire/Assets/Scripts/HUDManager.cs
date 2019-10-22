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
        PlatformerCharacter2D m_Character;

        // How many runes and the list to hold their positions
        // TODO: runecount is const by design at the moment, 
        // for our goal of having a game with a flexible inventory (being able to set down or carry a rune w/ you), this must eventually be changed to allow for some sort of "abilityManager" to hold the active runes
        // reminder: Linked Lists!
        public LinkedList<GameObject> m_lRuneList;

        // How fast to linearly interpolate for the rune icons
        public float lerpSpeed = 25.0f;

        // How far from player to display rune icons
        public float runeRadius = 4.0f;

        private void Awake()
        {
            // Initialize Rune List
            m_lRuneList = new LinkedList<GameObject>();

            // Grab ref to player character
            m_Character = GetComponent<PlatformerCharacter2D>();
        }

        void Start()
        {
            // Get all child game objects into our list
            
        }

        void Update()
        {
            // If flag enabled: 
            // - set runes to visible
            // - TODO: lerp from player to outer radius position
            if (m_Character.selectRunes)
            {
                int nRunes = m_lRuneList.Count;
                LinkedListNode<GameObject> curr = m_lRuneList.First;
                for (int i = 0; i < nRunes; ++i)
                {
                    float xPos = gameObject.transform.position.x + runeRadius * Mathf.Cos(Mathf.PI * (i + 1) / (nRunes + 1));
                    float yPos = gameObject.transform.position.y + runeRadius * Mathf.Sin(Mathf.PI * (i + 1) / (nRunes + 1));
                    Vector2 neededPosition = new Vector2(xPos, yPos);
                    curr.Value.transform.position = Vector2.Lerp(curr.Value.transform.position, neededPosition, lerpSpeed * Time.deltaTime);
                    curr = curr.Next;
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
            foreach (GameObject rune in m_lRuneList)
            {
                rune.transform.position = Vector2.Lerp(rune.transform.position, this.gameObject.transform.position, lerpSpeed * Time.deltaTime);
            }
        }

    }
}
