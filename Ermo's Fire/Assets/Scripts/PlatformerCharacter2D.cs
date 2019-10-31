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
        [SerializeField] private bool m_FacingRight = false;  // For determining which way the player is currently facing.
        //private bool fireRuneOn = false; // if fire rune is enabled or not  IIIIIIIIIIIIIIIIIIII -> Moved to Enum
        //private bool waterRuneOn = false; // if water rune is enabled or not IIIIIIIIIIIIIIIIIII -> Moved to Enum
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

        private Lives playerLives; //access to the players lives script

        private float invincibilityTimer; //time after taking damage that can take damage again clock
        public float invincibilitySeconds; //the editable time that a player can be immune to damage
        private bool fireImmunity; //bool to determine if player is immune to fire

        public bool inWater; //bool for whether we know that the player is in water or not
        private float normalGravity; //float to hold the normal gravity of the player
        private float waterGravity; //float that represents the value of gravity of the player when in water

        private Vector3 position, velocity;
        private float angularVelocity;
        private bool isCollidingBall;


        enum ActiveRune { fire, water, none };
        ActiveRune selectedRune;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            playerLives = GetComponent<Lives>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            selectedRune = ActiveRune.none;
            ActivateRunes();
            fireImmunity = false;
            invincibilityTimer = 0;

            //Setup the values for gravity
            normalGravity = 3.0f;
            waterGravity = 0.0f;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;
            fireImmunity = false;

            //find 2d mouse position 
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            mousePos = activeCam.ScreenToWorldPoint(mousePos);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //set gravity depending if you're in water or not
            if(!inWater)
            {
                //gameObject.GetComponent<m_Rigidbody2D>().;
                m_Grounded = true;

            }
            else
            {
                //Not be able to jump infinitely
            }
                


            //count down invincibility time
            if (invincibilityTimer > 0)
            {
                invincibilityTimer -= Time.deltaTime;
            }

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

            /*
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
            */ // Moved To Mouse Logic, no longer need test keys, enable if you want to mess with them as debug

            if (m_Selection)
            {
                //draw all runes
                DrawAll();

                

                //find the distances between each rune and the mouse
                foreach (GameObject thisRune in runes)
                {
                    if (thisRune.activeSelf)
                    {
                        Vector2 runePos = thisRune.transform.position;
                        runeDistances.Add(Vector2.Distance(mousePos, runePos));
                    }
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

                //sets to no runes active if there is no distances
                if(currentDistance == float.MaxValue)
                {
                    selectedRune = (ActiveRune)3;
                }
           
                //clear list for next update
                runeDistances.Clear();
            }

            //when no longer selecting rune make all but that rune invisible
            if(Input.GetMouseButtonUp(1))
            {
                DrawSelected();  
            }
            //activate rune primary based on selected rune
            if(runePrimaryActive)
            {
                switch(selectedRune)
                {
                    case (ActiveRune.fire):
                        SpawnFire(mousePos2D);
                        break;

                    case (ActiveRune.water):
                        if (Input.GetMouseButtonDown(0))
                        {
                            SpawnWater(mousePos2D);
                        }
                        break;
                }
            }

            //activate rune primary based on selected rune
            if (runeSecondaryActive)
            {
                switch (selectedRune)
                {
                    case (ActiveRune.fire):
                        fireImmunity = true;
                        break;

                    case (ActiveRune.water):

                        break;
                }
            }

            // Handling ball collision
            if (!isCollidingBall)
            {
                position = m_Rigidbody2D.position;
                velocity = m_Rigidbody2D.velocity;
                angularVelocity = m_Rigidbody2D.angularVelocity;
            }
        }

        private void LateUpdate()
        {
            if (isCollidingBall)
            {
                m_Rigidbody2D.position = position;
                m_Rigidbody2D.velocity = velocity;
                m_Rigidbody2D.angularVelocity = angularVelocity;
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

        /// <summary>
        ///  Method for flipping the sprite
        /// </summary>
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
        /// Did you also collect the water rune
        void OnCollisionEnter2D(Collision2D col)
        {
            // Other collider is for the incoming object
            // Regular collider is for the object itself

            //if you collect the fire rune
            if (col.collider.gameObject.tag == "Fire Rune")
            {
                Debug.Log("Collected the fire rune");
                hasFireRune = true;
                ActivateRunes();
                Destroy(col.collider.gameObject);
            }

            //if you collect the water rune
            if(col.collider.gameObject.tag == "Water Rune")
            {
                Debug.Log("Collected the water rune");
                hasWaterRune = true;
                ActivateRunes();
                Destroy(col.collider.gameObject);
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //take damage if in fire and not in fire mode
            if(collision.gameObject.tag == "Fire" && !fireImmunity)
            {
                if (invincibilityTimer <= 0)
                {
                    playerLives.TakeDamage(1);
                    invincibilityTimer = invincibilitySeconds;
                }
            }

            //set the bool value of inWater to true or false depending if player is colliding with water or not
            if(collision.gameObject.tag == "Water")
            {
                inWater = true;
            }

            // Colliding with the ball
            if (collision.gameObject.tag == "Ball")
            {
                isCollidingBall = true;

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Water")
            {
                inWater = false;
            }

            // Colliding with the ball
            if (collision.gameObject.tag == "Ball")
            {
                isCollidingBall = false;
            }
        }



        // Use fire if it's in fire rune mode
        private void SpawnFire(Vector2 mouse)
        {
            Vector3 firePosition = this.transform.position;
            float fireSeparator = 3.5f;

            Vector2 direction = (mouse - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
            firePosition += new Vector3(direction.x, direction.y, 0.0f) * fireSeparator;
            GameObject fireObj = Instantiate(firePre, firePosition, Quaternion.identity);
            fireObj.tag = "Fire";
            ConstantForce2D force = fireObj.GetComponent<ConstantForce2D>();
            force.force = new Vector2(direction.x * 10, (direction.y * 5) + 20);

        }

        // Use water if it's in water rune mode
        private void SpawnWater(Vector2 mouse)
        {
            Vector3 waterPosition = this.transform.position;
            float waterSeparator = 3.0f;

            Vector2 direction = (mouse - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
            waterPosition += new Vector3(direction.x, direction.y, 0.0f) * waterSeparator;
            GameObject waterObj = Instantiate(waterPre, waterPosition, Quaternion.identity);
            ConstantForce2D force = waterObj.GetComponent<ConstantForce2D>();
            force.force = new Vector2(direction.x * 100, direction.y * 100);
        }

        //determines which runes are contained by the player
        private void ActivateRunes()
        {
            runes[0].SetActive(hasFireRune);
            runes[1].SetActive(hasWaterRune);
            DrawSelected();
        }

        //draws the current selected rune
        private void DrawSelected()
        {
            //set only active rune as visible
            for (int i = 0; i < runes.Count; i++)
            {
                SpriteRenderer sprender = runes[i].GetComponent<SpriteRenderer>();
                GameObject runetext = runes[i].transform.GetChild(0).gameObject;
                if (i != (int)selectedRune)
                {
                    sprender.enabled = false;
                    runetext.SetActive(false);
                }
                else
                {
                    sprender.enabled = true;
                    runetext.SetActive(true);
                }
            }
        }

        //draws all runes
        private void DrawAll()
        {
            for (int i = 0; i < runes.Count; i++)
            {
                SpriteRenderer sprender = runes[i].GetComponent<SpriteRenderer>();
                GameObject runetext = runes[i].transform.GetChild(0).gameObject;            
                sprender.enabled = true;
                runetext.SetActive(true);
                
            }
        }
    }
}
