using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

    private Rigidbody2D player;
    public Collider2D col;
    public Transform groundCheck;
    public Transform jumpCheck;

    public SFX_Manager_Platform sounds;
    
    public LayerMask groundLayer;
    
    // Vida
    public bool isDead = false;

    // Victoria
    public bool win = false;

    // Animaciones
    [SerializeField]
    private Animator anim;
    private bool deathAnimPlayed = false;

    // Variables del movimiento
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 7f;

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
        transform.DOLocalMoveY(4,1).OnComplete(() => { 
            transform.DOLocalMoveY(-10,4);});

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
            sounds.playJumpSound();
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
        sounds.playWalkSound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDead = true;
            anim.SetBool("isDead", true);
            Invoke("DefeatCanvas", 3f);
            sounds.playDeathSound();
        } 
        if(collision.gameObject.tag == "FinishLine")
        {
            win = true;
            anim.SetBool("Win", true);
            Invoke("VictoryCanvas", 3f);
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

    public void DefeatCanvas()
    {
        defeat.SetActive(true);
        Invoke("Defeat", 3f);
        sounds.playDefeatSound();
    }

    public void VictoryCanvas()
    {
        victory.SetActive(true);
        Invoke("Victory", 3f);
        sounds.playVictorySound();
    }

    public void Defeat()
    {
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        PlayerPrefs.SetInt("minigameWin", 0);
        //SceneManager.UnloadSceneAsync("PlatformMinigame");
        SceneManager.LoadScene("TicTacToe_Server");
    }

    public void Victory()
    {
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        PlayerPrefs.SetInt("minigameWin", 1);
        //SceneManager.UnloadSceneAsync("PlatformMinigame");
        SceneManager.LoadScene("TicTacToe_Server");
    }
}
