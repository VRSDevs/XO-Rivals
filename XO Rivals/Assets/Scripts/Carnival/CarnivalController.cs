using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalController : MonoBehaviour
{

    public bool win = false;
    public bool lost = false;

    [SerializeField]
    private Transform bottom;
    [SerializeField]
    private Transform top;
    [SerializeField]
    private Transform bottomWin;
    [SerializeField]
    private Transform topWin;
    [SerializeField]
    private GameObject indicator;

    public bool goingUp = true;

    private float speed = 300;

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

    public void PressedButon()
    {
        speed = 0;
        CheckVictory();
    }

    public void CheckVictory()
    {
        if (indicator.transform.position.y < topWin.position.y && indicator.transform.position.y > bottomWin.position.y)
        {
            win = true;
        }
        else
        {
            lost = true;
        }
    }
}
