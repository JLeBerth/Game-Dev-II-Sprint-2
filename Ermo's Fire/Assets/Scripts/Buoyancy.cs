using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float UpwardForce = 12.72f;
    public GameObject SplashParticles;
    private bool isInWater = false;
    private bool splash = false;
    private Rigidbody2D rigidbody2D;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.tag == "Water")
        {
            isInWater = true;
            rigidbody2D.drag = 5f;
            rigidbody2D.angularDrag = 5f;
        }
        if (Other.tag == "Water" && !splash)
        {
            Instantiate(SplashParticles, transform.position, Quaternion.identity);
            splash = true;
        }

    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.tag == "Water")
        {
            isInWater = false;
            rigidbody2D.drag = 0.5f;
            rigidbody2D.angularDrag = 0.5f;
        }
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            Vector2 force = Vector2.up * UpwardForce;
            this.rigidbody2D.AddForce(force, ForceMode2D.Force);
            Debug.Log(isInWater);
        }
    }
}
