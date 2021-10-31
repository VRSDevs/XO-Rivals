using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager_Pistolero : MonoBehaviour
{
    private AudioSource managerPistolero;

    public AudioClip sonidoDisparo;
    public AudioClip sonidoRecarga;

    // Start is called before the first frame update
    void Start()
    {
        managerPistolero = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void playDisparo()
    {
        managerPistolero.PlayOneShot(sonidoDisparo, 2.0f);
    }

    public void playRecarga()
    {
        managerPistolero.PlayOneShot(sonidoRecarga, 2.0f);
    }

}
