using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaDetectScript : MonoBehaviour
{

    DetectPlayer detectScript;

    public GameObject player;
    public string enemigoSource;
    public bool creado = false;



    public void crear(GameObject enemy)
    {

        creado = true;
        detectScript = enemy.GetComponent<DetectPlayer>();

    }


    // Start is called before the first frame update
    void Start()
    {

       

        player = GameObject.FindWithTag("PlayerFantasma");

    }

    // Update is called once per frame
    void Update()
    {
        




    }
    private void FixedUpdate()
    {
        if (creado == true)
        {
            //MOVIMIENTO DE LA BALA HACIA EL JUGADOR 
            transform.Translate(Vector3.Normalize(player.transform.position - transform.position) * 0.5f);
        }
       
    }

    void OnCollisionEnter(Collision collision)
    {

        if (creado == true)
        {



            GameObject other = collision.gameObject;

            if (other.CompareTag("Enemy1"))//NO COLISIONA CON EL ENEMIGO
            {

            }
            else
            {



                if (other.CompareTag("PlayerFantasma"))//COLISION CON PLAYER
                {
                    detectScript.playerDetectedBala(true);
                }
                else//COLISION CON OBSTACULO
                {
                    detectScript.playerDetectedBala(false);
                }


                Destroy(gameObject);


            }


        }





    }


    //void OnTrigger2D(Collider2D collision)
    //{
    //    Debug.Log("colisdasdasdasd");
    //}


}

