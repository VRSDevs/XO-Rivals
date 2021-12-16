using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{





    public GameObject player;
    public float distanciaVision = 7;
    public bool veoPlayerRadio = false;
    public bool veoPlayerDistance = false;
    public bool linernaDetect = false;

    public GameObject prefabBala;
    public GameObject bala;

    public LayerMask layerMuro;


    void Start()
    {
        //Debug.Log(Vector3.Distance(player.transform.position,this.transform.position));
        StartCoroutine(creaBala());
        veoPlayerDistance = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError(veoPlayerDistance + "PLAYERVEO");
        if (Vector3.Distance(player.transform.position, this.transform.position) < distanciaVision)
        {
            veoPlayerRadio = true;
        }
        else
        {
            veoPlayerRadio = false;
        }


        Debug.Log(veoPlayerDistance + "PLAYER DISTANCE");
        Debug.Log(veoPlayerRadio + "PLAYER RADIO");
        Debug.Log(linernaDetect + "LINTERNA DETECT");

       // Debug.DrawRay(transform.position, player.transform.position - this.transform.position, Color.green);

    }


    public void playerDetectedBala(bool detected)
    {

        veoPlayerDistance = detected;
        Debug.LogError(veoPlayerDistance + "PLAYERVEO");


    }

    IEnumerator creaBala()
    {
        yield return new WaitForSeconds(0.2f);

        ////INSTANCIAR PREFAB
        //bala = Instantiate(prefabBala, new Vector3(this.transform.position.x, this.transform.position.y+1.7f, this.transform.position.z+1), Quaternion.identity);
        RaycastHit rayCast;
       
        if (Physics.Raycast(transform.position, player.transform.position - this.transform.position, out rayCast, 10000, layerMuro))
        {
            if (rayCast.collider.tag == "PlayerFantasma")
            {
                veoPlayerDistance = true;
            }
            else
            {
                veoPlayerDistance = false;
            }
            
        }


        //CODE
        StartCoroutine(creaBala());
    }


    public void playerDetectedLinterna(bool detect)//CUANDO ÑLA LINTERNA DETECTA AL JUGADOR
    {

        linernaDetect = detect;
        Debug.LogError(linernaDetect + "LINTERNADECTECT");

    }
    /*
    IEnumerator repetirDetectedLinterna() //Solo tiene en cuenta si esta en el radio
    {

        yield return new WaitForSeconds(2);


        if (veoPlayerRadio)//SE OLVIDA DE TI SI SALES DEL RADIO
        {
            GetComponentInParent<EnemyNavMesh>().playerDetectedLinternaNav(true);

            StartCoroutine(repetirDetectedLinterna());
        }
        else
        {

            GetComponentInParent<EnemyNavMesh>().playerDetectedLinternaNav(false);

        }


    }
    */


}
