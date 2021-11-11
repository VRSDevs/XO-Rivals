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
    private Rigidbody2D player;

    public GameObject sombra;

    private bool vivo = true;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sombra.transform.position = new Vector3(this.transform.position.x+0.9f, this.transform.position.y-0.87f, this.transform.position.z);

        if (vivo)
        {
            player.velocity = new Vector2(horizontal * speed, vertical * speed);
        }


    }

    private void FixedUpdate()
    {   

    }


    public void MoveHorizontal(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;
    }

    public void MoveVertical(InputAction.CallbackContext context)
    {
        vertical = context.ReadValue<Vector2>().y;
    }

}
