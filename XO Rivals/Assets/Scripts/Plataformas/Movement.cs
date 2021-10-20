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
    [SerializeField] private float Speed = 3, jumpSpeed;

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
        float movementInput = playerActionsController.Movement.Move.ReadValue<float>();
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * Speed * Time.deltaTime;
        transform.position = currentPosition;
    }

    public void Start()
    {
        playerActionsController.Movement.Jump.performed += _ => jump();
    }

    private void jump()
    {
        if (grounded = true)
        {
            body.velocity = new Vector2(body.velocity.x, speed);
            grounded = false;
        }
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
}