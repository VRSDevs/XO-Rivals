using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambioCreditos : MonoBehaviour
{

    

    public float speed = 3000;
    // Start is called before the first frame update

    public GameObject artButton;
    public GameObject progButton;
    public GameObject soundButton;

    public GameObject artMenu;
    public GameObject progMenu;
    public GameObject soundMenu;

 



    Vector2 initialArt;
    Vector2 initialProg;
    Vector2 initialSound;

    public GameObject targetObj;
    public Vector2 target;
    public GameObject objectToMove;
    bool moveR = false;
    bool moveL = false;


    int actualMenu = 0; //0 base--- 1 art --- 2prog --- 3sound
    


    void Start()
    {
        initialArt = artButton.transform.position;
        initialProg = progButton.transform.position;
        initialSound = soundButton.transform.position;
        //target = new Vector3(-225,artButton.transform.position.y);
        target = targetObj.transform.position;
        


    }

    // Update is called once per frame
    void Update()
    {
       

        if (moveR == true)
        {

            target = targetObj.transform.position;
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(objectToMove.transform.position, target, step);
            objectToMove.transform.position = aux;


            //CONTINUAMOS MOVIENDOLOS A PESAR DE QUE ESTE MOVER A TRUE


        }
        if (moveL == true)
        {
            /*
            if (objectToMove == artButton )
            {
                target = initialArt;
            }
            if (objectToMove == progButton)
            {
                target = initialProg;
            }
            if (objectToMove == soundButton)
            {
                target = initialSound;
            }
            */
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(artButton.transform.position, initialArt, step);
            artButton.transform.position = aux;

            float step2 = speed * Time.deltaTime;
            Vector2 aux2= Vector2.MoveTowards(progButton.transform.position, initialProg, step);
            progButton.transform.position = aux2;

            float step3 = speed * Time.deltaTime;
            Vector2 aux3 = Vector2.MoveTowards(soundButton.transform.position, initialSound, step);
            soundButton.transform.position = aux3;





        }
        
       


    }


    public void ExplainButton(string department)
    {
        //initialArt = artButton.transform.position;
        //initialProg = progButton.transform.position;
        //initialSound = soundButton.transform.position;

        if (department == "Art")
        {
            // artButton.transform.position = target;
            //artButton.transform.position = target;
            objectToMove = artButton;
            moveL = false;
            moveR = true;


            progButton.SetActive(false);
            soundButton.SetActive(false);
            actualMenu = 1;


            artMenu.SetActive(true);




        }
        else if (department == "Prog")
        {
            objectToMove = progButton;
            moveL = false;
            moveR = true;



            artButton.SetActive(false);
            soundButton.SetActive(false);
            actualMenu = 2;

            progMenu.SetActive(true);


        }
        else if (department == "Sound")
        {

            objectToMove = soundButton;
            moveL = false;
            moveR = true;


            artButton.SetActive(false);
            progButton.SetActive(false);
          //  soundButton.SetActive();

            actualMenu = 3;


            soundMenu.SetActive(true);


        }




    }
    public void UnExplainButton()
    {

        moveR = false;
        moveL = true;




        actualMenu = 0;

        artButton.SetActive(true);
        progButton.SetActive(true);
        soundButton.SetActive(true);



        artMenu.SetActive(false);
        progMenu.SetActive(false);
        soundMenu.SetActive(false);



    }

   

}
