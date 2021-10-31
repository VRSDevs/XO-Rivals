using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlBoton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private AudioSource sonidoPistolero;
    public Cronometro controlCrono;
    public GameObject botonExit;

    public AudioClip sonidoRecarga;
    public AudioClip sonidoDisparo; 

    // Start is called before the first frame update
    void Start()
    {
        sonidoPistolero = GetComponent<AudioSource>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        sonidoPistolero.PlayOneShot(sonidoRecarga, 1.0f);
        controlCrono.activarCrono();
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {      
        controlCrono.finCrono();
        sonidoPistolero.PlayOneShot(sonidoDisparo, 1.0f);
        this.gameObject.SetActive(false);
        botonExit.SetActive(true);
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
