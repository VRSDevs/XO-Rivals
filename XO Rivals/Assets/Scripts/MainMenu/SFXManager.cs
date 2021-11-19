using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource managerBoton;
    
    public AudioSource CookingMusic;
    public AudioSource unmanMusic;
    public AudioSource PlatformMusic;
    public AudioSource Tic_Tac_ToeMusic;
    public AudioSource ShopMusic;

    public AudioClip sonidoBoton1;
    public AudioClip sonidoBoton2;
    public AudioClip sonidoBoton3;
    public AudioClip sonidoBoton4; 

    // Start is called before the first frame update
    void Start()
    {
        managerBoton = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playSound1()
    {
        managerBoton.PlayOneShot(sonidoBoton1);
    }

    public void playSound2()
    {
        managerBoton.PlayOneShot(sonidoBoton2);
    }

    public void playSound3()
    {
        managerBoton.PlayOneShot(sonidoBoton3);
    }

    public void playSound4()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
}
