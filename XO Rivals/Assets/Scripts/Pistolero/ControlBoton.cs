using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlBoton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Cronometro controlCrono;
    public GameObject botonExit;
    public Button bangButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {        
        controlCrono.activarCrono();       
    }

    public void OnPointerUp(PointerEventData eventData)
    {     
        
        controlCrono.finCrono();

        this.gameObject.SetActive(false);
        //botonExit.SetActive(true);
       
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

}
