using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class RuneSelfDestroy : MonoBehaviour
{
    bool timeToDestroySelf;
    public GameObject player;
  
    // Start is called before the first frame update
    void Start()
    {
        timeToDestroySelf = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToDestroySelf)
        {
            Destroy(gameObject);
        }
    }

    // If player collides with the rune, set that bool to true because they got it now
    private void OnCollisionEnter2D(Collision2D col)
    {

        // Make sure it's the player first
        if(col.otherCollider.gameObject.tag == "Player")
        {
            if(gameObject.tag == "Fire Rune")
            {
                player.GetComponent<PlatformerCharacter2D>().hasFireRune = true;
            }
            else if(gameObject.tag == "Water Rune")
            {
                player.GetComponent<PlatformerCharacter2D>().hasWaterRune = true;
            }


            timeToDestroySelf = true;

            Debug.Log("collided with rune");
        }
    }
}
