using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Manager local del sonido
    private AudioSource managerSonido;

    //Pistas de audio 
    public AudioClip sonidoJump;
    public AudioClip sonidoMuerte;

    public string textValue;
    public TextMeshProUGUI textElement;
    
    [SerializeField] private float speed;
    private bool grounded;
    private Rigidbody2D body;
    private PlayerActionsController playerActionsController;
    [SerializeField] private float jumpSpeed = 4;
    public bool Victory = false;
    
    public Transform respawn;
    private Vector2 respawnPlayer;
    public GameObject player;
    private float movementInput;
    private bool isFacingRight = true;
    
    // Gamemanager
    private GameManager _gameManager;

    // Controles de movil
    [SerializeField]
    public GameObject leftButton;
    [SerializeField]
    public GameObject rightButton;
    [SerializeField]
    public GameObject jumpButton;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerActionsController = new PlayerActionsController();
    }
    private void FixedUpdate()
    {
        body.velocity = new Vector2(movementInput * speed, body.velocity.y);
    }

    public void OnEnable()
    {
        playerActionsController.Enable();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    public void OnDisable()
    {
        playerActionsController.Disable();
    }

    public void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        managerSonido = GetComponent<AudioSource>();
        if (!_gameManager.IsWebGLMobile)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            jumpButton.SetActive(false);

        } 
    }

    // Update is called once per frame
    void Update()
    {

        textElement.text = textValue;

        
        if (!isFacingRight && movementInput > 0f)
        {
            Flip();
        }
        else if (isFacingRight && movementInput < 0f)
        {
            Flip();
        }
    }
    public void Jump2(InputAction.CallbackContext context)
    {

        managerSonido.PlayOneShot(sonidoJump);

        if (context.performed)

        {
            body.velocity = new Vector2(body.velocity.x,jumpSpeed);            

        }

        if (context.canceled && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x,body.velocity.y * 0.5f);
            
        }
        
    }
    public void Jump()
    {
        body.velocity = Vector2.up * jumpSpeed;
        managerSonido.PlayOneShot(sonidoJump);
    }

    public void MoveLeft()
    {
        movementInput = -1;
    }
    
    public void MoveRight()
    {
        movementInput = 1;
    }

    public void Stop()
    {
        movementInput = 0;
    }
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>().x;
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Victoria");
            textValue = "Victory";
            Victory = true;
            OnDisable();

            FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
            PlayerPrefs.SetInt("minigameWin", 1);
            SceneManager.LoadScene("TicTacToe_Server");
        }
    }
private void OnCollisionEnter2D(Collision2D collision)
  {
      if (collision.gameObject.CompareTag ("Respawn")) {
                   player.transform.position = new Vector2(respawn.position.x,respawn.position.y);
                   managerSonido.PlayOneShot(sonidoMuerte);
        }

      if (collision.gameObject.CompareTag("Ground")) {
                grounded = true;
      }
    }
}