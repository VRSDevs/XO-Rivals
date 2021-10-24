using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
        
    public string textValue;
    public TextMeshProUGUI textElement;
    [SerializeField] private float speed;
    private bool grounded;
    private Rigidbody2D body;
    private PlayerActionsController playerActionsController;
    [SerializeField] private float Speed = 3, jumpSpeed = 4;
    public bool Victory = false;
    public Transform respawn;
    private Vector2 respawnPlayer;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerActionsController = new PlayerActionsController();
    }

    public void OnEnable()
    {
        playerActionsController.Enable();
    }


    public void OnDisable()
    {
        playerActionsController.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = textValue;
        
        float movementInput = playerActionsController.Movement.Move.ReadValue<float>();
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * Speed * Time.deltaTime;
        transform.position = currentPosition;

        if (grounded = true)
        {
            float jumpInput = playerActionsController.Movement.Jump.ReadValue<float>();
            Vector3 currentJump = transform.position;
            currentJump.y += jumpInput * jumpSpeed * Time.deltaTime;
            transform.position = currentJump;
            grounded = false;

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Victory"))
        {
            Debug.Log("Victoria");
            textValue = "Victory";
            Victory = true;
            OnDisable();
        }
    }

 
  private void OnCollisionEnter2D(Collision2D collision)
  {
        
      if (collision.gameObject.tag == "Muerte") {
                   player.transform.position = new Vector2(respawn.position.x,respawn.position.y);
      }

      if (collision.gameObject.tag == "Ground") {
                grounded = true;
      }
    }
}