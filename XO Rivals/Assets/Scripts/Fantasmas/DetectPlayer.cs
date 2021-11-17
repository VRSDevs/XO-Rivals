using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public float distanciaVision = 7;
    public bool veoPlayerRadio = false;
    public bool veoPlayerDistance = false;

    public GameObject prefabBala;
    public GameObject bala;
    

    void Start()
    {
        //Debug.Log(Vector3.Distance(player.transform.position,this.transform.position));
        StartCoroutine(creaBala());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < distanciaVision)
        {
            veoPlayerRadio = true;
        }
        else
        {
            veoPlayerRadio = false;
        }

       




    }


    public void playerDetectedBala(bool detected)
    {

        veoPlayerDistance = detected;



    }

    IEnumerator creaBala()
    {
        yield return new WaitForSeconds(0.2f);

        //INSTANCIAR PREFAB
        bala = Instantiate(prefabBala, new Vector3(this.transform.position.x, this.transform.position.y+1.7f, this.transform.position.z+1), Quaternion.identity);
        bala.GetComponent<BalaDetectScript>().crear(this.gameObject);


        //CODE
        StartCoroutine(creaBala());
    }


    public void playerDetectedLinterna()//CUANDO ÑLA LINTERNA DETECTA AL JUGADOR
    {

        if (veoPlayerDistance && veoPlayerRadio)
        {
            GetComponentInParent<EnemyNavMesh>().playerDetectedLinternaNav(true);

            StartCoroutine(repetirDetectedLinterna());
        }
        else
        {

            GetComponentInParent<EnemyNavMesh>().playerDetectedLinternaNav(false);
            
        }


    }

    IEnumerator repetirDetectedLinterna()
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



    }
