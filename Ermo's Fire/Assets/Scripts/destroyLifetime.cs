using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyLifetime : MonoBehaviour
{
    private float timePassed; //checks amount of time left till destroy

    public float timeToDie; //declared time till death

    private void Awake()
    {
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timeToDie < timePassed)
        {
            Destroy(this.gameObject);
        }
    }
}
