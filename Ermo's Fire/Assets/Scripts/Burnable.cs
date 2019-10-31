using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public float timeToBurn;
    private float changePerTime; //the amount of red should change per second so it reaches max red at destruction
    private SpriteRenderer sprender; //sprite renderer to change color
    private Renderer renderer; //renderer of a physical object to change color
    private float newSpriteTime; //time inbetween new sprites being made
    private Color renderColor;
    GameObject fireSprite;
    bool burning;

    // Start is called before the first frame update
    void Awake()
    {
        newSpriteTime = timeToBurn / 3;
        changePerTime = (256 / timeToBurn) / 5;
        burning = false;
        if(this.gameObject.GetComponent<SpriteRenderer>())
        {
            sprender = this.gameObject.GetComponent<SpriteRenderer>();
        }
        if (this.gameObject.GetComponent<Renderer>())
        {
            renderer = this.gameObject.GetComponent<Renderer>();
        }
        fireSprite = Resources.Load("burnFire", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if(burning)
        {
            timeToBurn -= Time.deltaTime;
            newSpriteTime -= Time.deltaTime;
            if(newSpriteTime < 0)
            {
                CreateFireSprite();
                newSpriteTime = timeToBurn / 3.0f;
            }

            renderColor.r += changePerTime * Time.deltaTime;
            if (renderer != null)
            {
                //renderer.material.color = renderColor;
            }
            else
            {
                
                sprender.color = renderColor;
            }

            
        }

        if(timeToBurn <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        // Other collider is for the incoming object
        // Regular collider is for the object itself
        if (col.gameObject.tag == "Fire")
        {
            burning = true;       
        }
    }

    private void CreateFireSprite()
    {
        Transform thisTransform = this.gameObject.transform;
        float width = thisTransform.localScale.x / 2;
        float height = thisTransform.localScale.y / 2;

        float xOff = Random.Range(-width, width);
        float yOff = Random.Range(-height, height);
        Vector3 position = new Vector3(thisTransform.position.x + xOff, thisTransform.position.y + yOff, thisTransform.position.z - 1);
        destroyLifetime timeLeft = fireSprite.GetComponent<destroyLifetime>();
        timeLeft.timeToDie = timeToBurn;
        Instantiate(fireSprite, position, Quaternion.identity);


    }
}
