using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{

    //POSICION NEGRO = x:Plyaer +0.7    y:pñlayer -0.87
    [SerializeField] private float speed;
    private PlayerActionsController playerActionsController;
    private float movementInputx;
    private float movementInputy;
    private Rigidbody2D body;

    // Controles de movil
    [SerializeField]
    public GameObject leftButton;
    [SerializeField]
    public GameObject rightButton;
    [SerializeField]
    public GameObject topButton;
    [SerializeField]
    public GameObject botButton;

    public GameObject sombra;

    private void Awake()
    {
        playerActionsController = new PlayerActionsController();
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // sombra.transform.position = new Vector3(this.transform.position.x+0.9f, this.transform.position.y-0.87f, this.transform.position.z);
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(movementInputx * speed, movementInputy*speed);

    }

    public void OnEnable()
    {
        playerActionsController.Enable();
    }

    public void OnDisable()
    {
        playerActionsController.Disable();
    }



    //public void MoveLeft()
    //{
    //    movementInputx = -1;
    //}

    //public void MoveRight()
    //{
    //    movementInputx = 1;
    //}

    //public void MoveTop()
    //{
    //    movementInputy = 1;
    //}

    //public void MoveBot()
    //{
    //    movementInputy = -1;
    //}

    //public void Stop()
    //{
    //    movementInputx = 0;
    //    movementInputy = 0;
    //}

    //public void Move(InputAction.CallbackContext context)
    //{
    //    movementInputx = context.ReadValue<Vector2>().x;
    //    movementInputy = context.ReadValue<Vector2>().y;
    //}

}
