using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalController : MonoBehaviour
{

    [SerializeField]
    private Transform bottom;
    [SerializeField]
    private Transform top;
    [SerializeField]
    private GameObject indicator;

    private float posY;

    public bool pressed = false;

    public bool goingUp = true;

    private float speed = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BarMovement();
        if (indicator.transform.position.y > top.position.y - 10)
        {
            goingUp = false;
        }

        if (indicator.transform.position.y < bottom.position.y + 10)
        {
            goingUp = true;
        }
    }

    private void BarMovement()
    {
        if (goingUp)
        {      
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(indicator.transform.position, top.position, step);
            indicator.transform.position = aux;
        }
        else
        {
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(indicator.transform.position, bottom.position, step);
            indicator.transform.position = aux;
        }
    }

    void PressedButon()
    {
        pressed = true;
    }
}
