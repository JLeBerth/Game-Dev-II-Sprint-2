using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnCollision : MonoBehaviour
{
    ////tells object to die when it hits things
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Destroy(this.gameObject);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Destroy(this.gameObject);
    //}

    private void Update()
    {
        Destroy(gameObject, 1f);
    }
}
