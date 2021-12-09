using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{

    //POSICION NEGRO = x:Plyaer +0.7    y:p�layer -0.87
    [SerializeField] private float speed = 8;
    public float horizontal;
    public float vertical;
    private Rigidbody player;

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

    [SerializeField]
    private Animator anim;

    private bool vivo = true;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
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
        horizontal = -1;
        transform.rotation = quatL;
    }

    public void MoveRight()
    {
        horizontal = 1;
        transform.rotation = quatR;
    }

    public void Stop()
    {
        horizontal = 0;
        vertical = 0;
    }
    public void MoveDown()
    {
        vertical = -1;
        transform.rotation = quatB;
    }
    public void MoveUp()
    {
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
