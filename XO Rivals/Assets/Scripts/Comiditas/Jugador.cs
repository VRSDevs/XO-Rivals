using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Jugador : MonoBehaviour
{

    ComidaController controlador;

    private Rigidbody2D player;
    public Collider2D col;

    private float horizontal;
    private float speed = 6f;
    private bool isFacingRight = true;

    // Animaciones
    [SerializeField]
    private Animator anim;
    private bool deathAnimPlayed = false;

    

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

        if (Input.GetKeyUp("a"))
        {
            horizontal = 0;
        }

        if (Input.GetKeyUp("d"))
        {
            horizontal = 0;
        }

        if (Input.GetKeyDown("a") || Input.GetKey("a"))
        {
            horizontal = -1;
        }       

        if (Input.GetKeyDown("d") || Input.GetKey("d"))
        {
            horizontal = 1;
        }
            

        player.velocity = new Vector2(horizontal * speed, player.velocity.y);
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        } 
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        if (controlador.lost == true && !deathAnimPlayed)
        {
            deathAnimPlayed = true;
            anim.SetBool("Dead", controlador.lost);
            DeathAnimation();
        }

        anim.SetFloat("Speed", Mathf.Abs(horizontal));
        
        anim.SetBool("Win",controlador.win);

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

        if (collision.gameObject.tag == "panArriba")
        {
            Destroy(collision.gameObject);
            controlador.recibirComida(5);

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

    private void DeathAnimation()
    {
        col.enabled = false;
        transform.DOMoveY(1, 1).OnComplete(() => {
            transform.DOMoveY(-10, 4);
        });

    }
}
