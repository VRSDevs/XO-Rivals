using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaDetectScript : MonoBehaviour
{
    public GameObject detectEnemy;
    DetectPlayer detectScript;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        detectEnemy = GameObject.FindWithTag("Enemy1");
        detectScript = detectEnemy.GetComponent<DetectPlayer>();

        player = GameObject.FindWithTag("PlayerFantasma");

    }

    // Update is called once per frame
    void Update()
    {
        




    }
    private void FixedUpdate()
    {
        //MOVIMIENTO DE LA BALA HACIA EL JUGADOR 
        transform.Translate(Vector3.Normalize(player.transform.position - transform.position) * 0.5f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("coli");
        GameObject other = collision.gameObject;

        if (other.CompareTag("Enemy1"))//NO COLISIONA CON EL ENEMIGO
        {

        }
        else
        {

            Debug.Log("coliMenda");

            if (other.CompareTag("PlayerFantasma"))//COLISION CON PLAYER
            {
                detectScript.playerDetected(true);
            }
            else//COLISION CON OBSTACULO
            {
                detectScript.playerDetected(false);
            }


            Destroy(gameObject);
            Debug.Log("DESTRUYO");

        }




    }


    //void OnTrigger2D(Collider2D collision)
    //{
    //    Debug.Log("colisdasdasdasd");
    //}


}

