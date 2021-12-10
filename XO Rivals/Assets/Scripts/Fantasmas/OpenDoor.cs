using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {



        if (collision.gameObject.name == "ColliderPuerta")
        {

            collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);


        }

    }
    private void OnTriggerExit(Collider collision)
    {



        if (collision.gameObject.name == "ColliderPuerta")
        {

            collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);


        }

    }


}
