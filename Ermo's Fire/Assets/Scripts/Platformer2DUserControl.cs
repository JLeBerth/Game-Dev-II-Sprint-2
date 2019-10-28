using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        public bool m_Selection;
        /*{
            get
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    return true;
                }
                return false;
            }
        }*/


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
       
            //detect if rune select is up
            m_Selection = Input.GetMouseButton(1);

            //detect if using primary ability
             m_Character.runePrimaryActive = Input.GetMouseButton(0);

            //detect if using secondary
            m_Character.runeSecondaryActive = Input.GetKey(KeyCode.LeftShift);

            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
