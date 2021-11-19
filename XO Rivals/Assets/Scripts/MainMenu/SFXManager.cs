using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource managerBoton;
    
    public AudioClip CookingMusic;
    public AudioClip GunmanMusic;
    public AudioClip PlatformMusic;
    public AudioClip Tic_Tac_ToeMusic;
    public AudioClip ShopMusic;

    public AudioClip sonidoBoton1;
    public AudioClip sonidoBoton2;
    public AudioClip sonidoBoton3;
    public AudioClip sonidoBoton4;
    
    public AudioClip DeathSound;
    public AudioClip VictorySound;
    public AudioClip DefeatSound;
    public AudioClip WalkSound;
    public AudioClip ReloadSound;
    public AudioClip ShotSound;
    public AudioClip ChipSound;
    public AudioClip CookingSound;


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
    
    
    public void playDeathSound()
    {
        managerBoton.PlayOneShot(DeathSound);
    }
    public void playVictorySound()
    {
        managerBoton.PlayOneShot(VictorySound);
    }
    public void playDefeatSound()
    {
        managerBoton.PlayOneShot(DefeatSound);
    }
    public void playWalkSound()
    {
        managerBoton.PlayOneShot(WalkSound);
    }
    public void playReloadSound()
    {
        managerBoton.PlayOneShot(ReloadSound);
    }
    public void playShotSound()
    {
        managerBoton.PlayOneShot(ShotSound);
    }
    public void playChipSound()
    {
        managerBoton.PlayOneShot(ChipSound);
    }
    public void playCookingSound()
    {
        managerBoton.PlayOneShot(CookingSound);
    }
}
