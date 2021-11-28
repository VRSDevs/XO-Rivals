using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{

    //POSICION NEGRO = x:Plyaer +0.7    y:p�layer -0.87
    [SerializeField] private float speed = 8;
    private float horizontal;
    private float vertical;
    private Rigidbody player;

    private Quaternion quatR = new Quaternion(0.7071068f,0,0, 0.7071068f);
    private Quaternion quatL = new Quaternion(0, -0.7071068f, 0.7071068f,0);
    private Quaternion quatT = new Quaternion(0.5000001f,-0.5000001f, 0.5000001f, 0.5000001f);
    private Quaternion quatB = new Quaternion(-0.5000001f, -0.5000001f, 0.5000001f, -0.5000001f);



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
        }


    }

    private void FixedUpdate()
    {   

    }


    public void MoveHorizontal(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;
        if (horizontal == 1)
        {
            transform.rotation = quatR;
        }
        if (horizontal == -1)
        {
            transform.rotation = quatL;
        }

    }

    public void MoveVertical(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>().y;
        if (vertical == 1)
        {
            transform.rotation = quatT;
        }
        if (vertical == -1)
        {
            transform.rotation = quatB;
        }
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
}
