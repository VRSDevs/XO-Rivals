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
    private bool isFacingRight = true;

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

        player.velocity = new Vector2(horizontal * speed, player.velocity.y);
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        } 
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void MoveLeft()
    {
        horizontal = -1;
    }

    public void MoveRight()
    {
        horizontal = 1;
    }

    public void Stop()
    {
        horizontal = 0;   
    }
}
