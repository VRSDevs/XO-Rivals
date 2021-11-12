using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "carne")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "lechuga")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "queso")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "pan")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "panArriba")
        {
            Destroy(collision.gameObject);
        }
    }
}
