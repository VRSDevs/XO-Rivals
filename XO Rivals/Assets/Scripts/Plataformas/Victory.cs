using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Ganaste wey");
            
            
        }
        
        if (other.gameObject.tag == "PLAYER")
        {
            Debug.Log("Ganaste wey");
            
        }
    }
}
