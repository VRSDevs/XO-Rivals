using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinternaColision : MonoBehaviour
{


    public DetectPlayer enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "LinternaColision")
        {

            enemyScript.playerDetectedLinterna();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "LinternaColision")
        {

            enemyScript.playerDetectedLinterna();
        }

    }


}
