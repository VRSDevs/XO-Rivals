using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{

    ComidaController controlador;

    private Rigidbody2D player;

    private float horizontal;
    private float speed = 8f;


    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        controlador = FindObjectOfType<ComidaController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        player.velocity = new Vector2(horizontalInput * speed, player.velocity.y);
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
