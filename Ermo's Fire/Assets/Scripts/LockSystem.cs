using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private bool isButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonPressed)
        {
            StartCoroutine(Unlock());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            GetComponent<Animator>().SetBool("IsButtonPressed", true);
            isButtonPressed = true;
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForSeconds(2f);
        lockPrefab.GetComponent<Animator>().SetBool("IsButtonPressed", true);
    }
}
