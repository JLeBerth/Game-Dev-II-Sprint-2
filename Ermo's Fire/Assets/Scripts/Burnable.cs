using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public float timeToBurn;
    bool burning;

    // Start is called before the first frame update
    void Start()
    {
        timeToBurn = 3.0f;
        burning = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(burning)
        {
            timeToBurn -= Time.deltaTime;
        }

        if(timeToBurn <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Object collided");

        // Other collider is for the incoming object
        // Regular collider is for the object itself
        if (col.gameObject.tag == "Fire")
        {
            burning = true;
            Debug.Log("Object is burning now");
        }
    }
}
