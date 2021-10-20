using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool grounded;
    private Rigidbody2D body;
    private PlayerActionsController playerActionsController;

    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerActionsController = new PlayerActionsController();
    }

    private void OnEnable()
    {
       playerActionsController.Enable();
    }


    private void OnDisable()
    {
       playerActionsController.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
        playerActionsController.Movement
        
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput*speed,body.velocity.y);
        if (Input.GetKey((KeyCode.Space))&&grounded)
        {
            jump();
        }

      
    }

    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true; 
        }
        
        if (collision.gameObject.tag == "Victory")
        {
            Debug.Log("Ganaste wey");
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Victory")
        {
            Debug.Log("Ganaste wey");
        }
        
    }
}
