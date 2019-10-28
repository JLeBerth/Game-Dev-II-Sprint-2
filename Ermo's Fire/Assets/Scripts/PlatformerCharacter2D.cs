using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        public bool selectRunes = false; //For determing if the player is selecting runes
        private bool fireRuneOn = false; // if fire rune is enabled or not
        private bool waterRuneOn = false; // if water rune is enabled or not
        public bool hasFireRune = false; // no fire rune at the start
        public bool hasWaterRune = false; // no water rune at the start
        public GameObject firePre;
        public GameObject waterPre;

        public bool runePrimaryActive = false; //boolean value that determines if the primary is being used
        public bool runeSecondaryActive = false; //boolean value that determines if the secondary is being used
        public bool m_Selection = false; //boolean value that determines if runes are being selected

        public List<GameObject> runes = new List<GameObject>(); //list of all runes
        private List<float> runeDistances = new List<float>(); //list of distances from the body to the runes

        public Camera activeCam; //the active camera in the scene

        enum ActiveRune { fire, water };
        ActiveRune selectedRune = ActiveRune.fire;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            // fire rune enabled for testing purposes
            fireRuneOn = true;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            if (selectRunes)
            {
                //put in rune select code here
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                fireRuneOn = true;
                waterRuneOn = false;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                fireRuneOn = false;
                waterRuneOn = true;
            }

            if(m_Selection)
            {
                //find 2d mouse position 
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10.0f;
                mousePos = activeCam.ScreenToWorldPoint(mousePos);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                Debug.Log("Mouse Position: " + mousePos.ToString());
                //find the distances between each rune and the mouse
                foreach (GameObject thisRune in runes)
                {
                    Vector2 runePos = thisRune.transform.position;
                    Debug.Log("Rune Position: " + runePos.ToString());
                    runeDistances.Add(Vector2.Distance(mousePos, runePos));
                }

                float currentDistance = float.MaxValue;
                //set the active rune to the current closest
                for(int i = 0; i < runeDistances.Count; i++)
                {
                    Debug.Log("Rune Distance: " + runeDistances[i].ToString());
                    if (runeDistances[i] < currentDistance)
                    {
                        selectedRune = (ActiveRune)i;
                        currentDistance = runeDistances[i];

                        Debug.Log(selectedRune.ToString());
                    }
                }
                runeDistances.Clear();
            }

            //activate rune primary based on selected rune
            if(runePrimaryActive)
            {
                switch(selectedRune)
                {
                    case (ActiveRune.fire):
                        SpawnFire();
                        break;

                    case (ActiveRune.water):
                        break;
                }
            }

            //activate rune primary based on selected rune
            if (runeSecondaryActive)
            {
                switch (selectedRune)
                {
                    case (ActiveRune.fire):
                        
                        break;

                    case (ActiveRune.water):

                        break;
                }
            }
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            foreach(GameObject thisRune in runes)
            {
                Vector3 runeScale = thisRune.transform.localScale;
                runeScale.x *= -1;
                thisRune.transform.localScale = runeScale;
            }
        }

        /// Did you collect the fire rune
        void OnCollisionEnter2D(Collision2D col)
        {
            // Other collider is for the incoming object
            // Regular collider is for the object itself
            if (col.otherCollider.gameObject.tag == "Fire Rune")
            {
                Debug.Log("Collected the fire rune");
            }
        }


        // Use fire if it's in fire rune mode
        private void SpawnFire()
        {
            Vector3 firePosition = this.transform.position;
            float fireSeparator = 6;
            if(m_FacingRight)
            {
                firePosition.x += fireSeparator;
                GameObject fireObj = Instantiate(firePre, firePosition, Quaternion.identity);
                fireObj.tag = "Fire";
            }
            else
            {
                firePosition.x -= fireSeparator;
                GameObject fireObj = Instantiate(firePre, firePosition, Quaternion.identity);
                fireObj.tag = "Fire";
            }
        }

        // Use water if it's in water rune mode
        private void SpawnWater()
        {
           
        }
    }
}
