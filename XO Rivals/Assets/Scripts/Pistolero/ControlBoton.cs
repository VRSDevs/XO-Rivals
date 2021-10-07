using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlBoton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    public Cronometro controlCrono;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
