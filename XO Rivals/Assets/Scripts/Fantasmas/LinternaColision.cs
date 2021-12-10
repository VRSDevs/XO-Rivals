using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinternaColision : MonoBehaviour
{


    public List<DetectPlayer> enemigos;

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

        if (other.gameObject.name == "LinternaColision")
        {

            enemigos[0].playerDetectedLinterna(true);
        }else if (other.gameObject.name == "LinternaColision2")
        {
            enemigos[1].playerDetectedLinterna(true);
        }
        if (other.gameObject.name == "LinternaColision3")
        {
            enemigos[2].playerDetectedLinterna(true);
        }
        if (other.gameObject.name == "LinternaColision4")
        {
            enemigos[3].playerDetectedLinterna(true);
        }




    }
    /*
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "LinternaColision")
        {

            enemigos[0].playerDetectedLinterna();
        }
        else if (other.gameObject.name == "LinternaColision2")
        {
            enemigos[1].playerDetectedLinterna();
        }
        if (other.gameObject.name == "LinternaColision3")
        {
            enemigos[2].playerDetectedLinterna();
        }
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LinternaColision")
        {

            enemigos[0].playerDetectedLinterna(false);
        }
        else if (other.gameObject.name == "LinternaColision2")
        {
            enemigos[1].playerDetectedLinterna(false);
        }
        if (other.gameObject.name == "LinternaColision3")
        {
            enemigos[2].playerDetectedLinterna(false);
        }
        if (other.gameObject.name == "LinternaColision4")
        {
            enemigos[3].playerDetectedLinterna(false);
        }


    }


}
