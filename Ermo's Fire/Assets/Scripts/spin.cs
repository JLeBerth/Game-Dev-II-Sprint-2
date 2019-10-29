using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    private Transform thisTransform;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = this.gameObject.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        thisTransform.Rotate(new Vector3(0, 0, 10));
    }
}
