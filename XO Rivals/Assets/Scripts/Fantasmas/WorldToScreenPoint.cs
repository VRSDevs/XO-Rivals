using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreenPoint : MonoBehaviour
{

    public float positionX;
    public float positionY;
    public GameObject cam;
    public PostEddectAgujeroNegro camara;


    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {
        
        positionX = cam.GetComponent<Camera>().WorldToScreenPoint(this.transform.position).x;
        positionY = cam.GetComponent<Camera>().WorldToScreenPoint(this.transform.position).y;
        positionY = Screen.currentResolution.height-positionY;

        //ENVIAMOS LA POSICION EN LA CAMARA A LA CAMARA PARA QUE LA APLIQUE AL SHADER
        camara.SetPosicionPlayer(positionX,positionY);
    }

}
