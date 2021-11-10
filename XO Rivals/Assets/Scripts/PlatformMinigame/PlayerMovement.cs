using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    

    private Rigidbody2D player;
    public Collider2D col;
    public Transform groundCheck;
    public Transform jumpCheck;

    public LayerMask groundLayer;

    // Vida
    public bool isDead = false;

    // Victoria
    private bool win = false;

    // Animaciones
    [SerializeField]
    private Animator anim;
    private bool deathAnimPlayed = false;

    // Variables del movimiento
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 8f;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (win)
        {
            player.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        if (!isDead && !win) 
        {
            player.velocity = new Vector2(horizontal * speed, player.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));

            if (IsJumping())
            {
                anim.SetBool("isJumping", false);
            }

        } 
        else if(!deathAnimPlayed && isDead)
        {
            
            deathAnimPlayed = true;
            Invoke("DeathAnimation",0.2f);
        }
    }

    private void DeathAnimation()
    {
        col.enabled = false;
        transform.DOMoveY(1,1).OnComplete(() => { 
            transform.DOMoveY(-10,4);});
        
    }

    private bool IsJumping()
    {
        return Physics2D.OverlapCircle(jumpCheck.position, 0.1f, groundLayer);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isDead)
        {
            player.velocity = new Vector2(player.velocity.x, jumpingPower);
            anim.SetBool("isJumping", true);
        }

    }
    public void JumpPhone()
    {
        if (IsGrounded() && !isDead)
        {
            player.velocity = new Vector2(player.velocity.x, jumpingPower);
            anim.SetBool("isJumping", true);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x < 0) { return; }
        horizontal = context.ReadValue<Vector2>().x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDead = true;
            anim.SetBool("isDead", true);
        } 
        if(collision.gameObject.tag == "FinishLine")
        {
            win = true;
            anim.SetBool("Win", true);
        }
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
