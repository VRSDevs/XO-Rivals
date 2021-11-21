using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource managerBoton;

    public float volumenSFX;
    public float volumenMusica;

    public AudioClip CookingMusic;
    public AudioClip GunmanMusic;
    public AudioClip PlatformMusic;
    public AudioClip Tic_Tac_ToeMusic;
    public AudioClip ShopMusic;

    public AudioClip SelectionButton1;
    public AudioClip SelectionButton2;
    public AudioClip SelectionButton3;
    public AudioClip SelectionButton4;
    
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


    public void playSelectionButton1Sound()
    {
        managerBoton.PlayOneShot(SelectionButton1, volumenSFX);
    }

    public void playSelectionButton2Sound()
    {
        managerBoton.PlayOneShot(SelectionButton2, volumenSFX);
    }

    public void playSelectionButton3Sound()
    {
        managerBoton.PlayOneShot(SelectionButton3, volumenSFX);
    }

    public void playSelectionButton4Sound()
    {
        managerBoton.PlayOneShot(SelectionButton4, volumenSFX);
    }
    
    
    public void playDeathSound()
    {
        managerBoton.PlayOneShot(DeathSound, volumenSFX);
    }
    public void playVictorySound()
    {
        managerBoton.PlayOneShot(VictorySound, volumenSFX);
    }
    public void playDefeatSound()
    {
        managerBoton.PlayOneShot(DefeatSound, volumenSFX);
    }
    public void playWalkSound()
    {
        managerBoton.PlayOneShot(WalkSound, volumenSFX);
    }
    public void playReloadSound()
    {
        managerBoton.PlayOneShot(ReloadSound, volumenSFX);
    }
    public void playShotSound()
    {
        managerBoton.PlayOneShot(ShotSound, volumenSFX);
    }
    public void playChipSound()
    {
        managerBoton.PlayOneShot(ChipSound, volumenSFX);
    }
    public void playCookingSound()
    {
        managerBoton.PlayOneShot(CookingSound, volumenSFX);
    }
}
