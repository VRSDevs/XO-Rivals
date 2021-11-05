using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D player;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Animaciones
    [SerializeField]
    private Animator anim;
    private string currentState;
    const string IDLE = "Idle";
    const string WALK = "Saminar";
    const string JUMP = "Salto";
    const string FALL = "Caida";

    // Variables del movimiento
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 8f;
    private bool isFalling = false;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        player.velocity = new Vector2(horizontal * speed, player.velocity.y);
        AnimationController();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpingPower);
            ChangeAnimationState(JUMP);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x < 0) { return; }
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }

    public void Falling()
    {
        if (player.velocity.y < 0)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }
    }

    public void AnimationController()
    {
        if (isFalling == true)
        {
            ChangeAnimationState(FALL);
        }

        if (IsGrounded() && player.velocity.x > 0)
        {
            ChangeAnimationState(WALK);
        } else if (IsGrounded() && player.velocity.x == 0)
        {
            ChangeAnimationState(IDLE);
        }
    }

}
