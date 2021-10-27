using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{



    ComidaController controlador;



    // Start is called before the first frame update
    void Start()
    {
        controlador = FindObjectOfType<ComidaController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "carne")
        {
            Destroy(collision.gameObject);
            controlador.recibirComida(3);
            
        }

        if (collision.gameObject.tag == "lechuga")
        {
            Destroy(collision.gameObject);
            controlador.recibirComida(4);
            
        }

        if (collision.gameObject.tag == "queso")
        {
            Destroy(collision.gameObject);
            controlador.recibirComida(2);
            
        }

        if (collision.gameObject.tag == "pan")
        {
            Destroy(collision.gameObject);
            controlador.recibirComida(1);
            
        }
    }
}
