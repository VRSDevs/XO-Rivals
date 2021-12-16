using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MovePlayer : MonoBehaviour
{

    //POSICION NEGRO = x:Plyaer +0.7    y:p�layer -0.87
    [SerializeField] private float speed = 8;
    public float horizontal;
    public float vertical;
    private Rigidbody player;
    private bool botonPulsado = false;

    //ROTACIONES NORMALES
    private Quaternion quatR = new Quaternion(0.7071068f,0,0, 0.7071068f);
    private Quaternion quatL = new Quaternion(0, -0.7071068f, 0.7071068f,0);
    private Quaternion quatT = new Quaternion(0.5000001f,-0.5000001f, 0.5000001f, 0.5000001f);
    private Quaternion quatB = new Quaternion(-0.5000001f, -0.5000001f, 0.5000001f, -0.5000001f);

    //ROTACIONES DIAGONALES
    private Quaternion quatBR = new Quaternion(0.6532816f, 0.2705981f, -0.2705981f, 0.6532816f);
    private Quaternion quatTR = new Quaternion(0.6532816f, -0.2705981f, 0.2705981f, 0.6532816f);
    private Quaternion quatBL = new Quaternion(-0.2705979f, -0.6532816f, 0.6532816f, -0.2705979f);
    private Quaternion quatTL = new Quaternion(0.2705981f, -0.6532816f, 0.6532816f, 0.2705981f);

    private Animator anim;
    [SerializeField]private Animator animX;
    [SerializeField] private Animator animO;

    public GameObject xSprite;
    public GameObject oSprite;

    public Match thisMatch;

    public PlayerInfo localPlayer;
    private GameManager _gameManager;

    private bool vivo = true;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = _gameManager.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {

        // Recoger de quien es el turno y activar los personajes necesarios
        if (thisMatch.PlayerOName == localPlayer.Name) //TURNO O
        {
            xSprite.SetActive(false);

            anim = animO;
        }
        else
        {
            oSprite.SetActive(false);                   //TURNO X
            anim = animX;
        }



    }

    // Update is called once per frame
    void Update()
    {
        

        if (vivo)
        {
            player.velocity = new Vector3(horizontal * speed, 0, vertical * speed);

            if (player.velocity.sqrMagnitude == 0)
            {
                anim.speed = 0;
            } else
            {
                anim.speed = 1.3f;
            }
        }


    }

    private void FixedUpdate()
    {
   
    }


    public void MoveHorizontal(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;
        rotarPersonaje();


    }

    public void MoveVertical(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>().y;
        rotarPersonaje();

    }

    public void MoveLeft()
    {
     
            botonPulsado = true;
            horizontal = -1;
            transform.rotation = quatL;
        
       
    }

    public void MoveRight()
    {
       
            botonPulsado = true;
            horizontal = 1;
            transform.rotation = quatR;
        
       
    }

    public void StopHor()
    {
        
            botonPulsado = false;
            horizontal = 0;
        
       
        
    }

    public void StopVer()
    {

            botonPulsado = false;
            vertical = 0;
        
       
    }
    public void MoveDown()
    {
        
            botonPulsado = true;
            vertical = -1;
            transform.rotation = quatB;
        
       
    }
    public void MoveUp()
    {
        
            botonPulsado = true;
            vertical = 1;
            transform.rotation = quatT;
        
        
    }

    private void rotarPersonaje()
    {
        //ARRIBA
        if (vertical == 1 && horizontal == 0)
        {
            transform.rotation = quatT;
        }
        //ABAJO
        if (vertical == -1 && horizontal == 0)
        {
            transform.rotation = quatB;
        }

        //ABAJO DERECHA
        if (vertical == -0.707107f && horizontal == 0.707107f)
        {
            transform.rotation = quatBR;
        }

        //ABAJO IZQUIERDA
        if (vertical == -0.707107f && horizontal == -0.707107f)
        {
            transform.rotation = quatBL;
        }

        //ARRIBA DERECHA
        if (vertical == 0.707107f && horizontal == 0.707107f)
        {
            transform.rotation = quatTR;
        }

        //ARRIBA IZQUIERDA
        if (vertical == 0.707107f && horizontal == -0.707107f)
        {
            transform.rotation = quatTL;
        }
        //DERECHA
        if (horizontal == 1 && vertical == 0)
        {
            transform.rotation = quatR;
        }

        //IZQUIERDA
        if (horizontal == -1 && vertical == 0)
        {
            transform.rotation = quatL;
        }
    }

}
