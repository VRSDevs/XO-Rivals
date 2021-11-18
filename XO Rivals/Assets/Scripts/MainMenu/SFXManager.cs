using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource managerBoton;

    public AudioClip selectBotton1;
    public AudioClip selectBotton2;
    public AudioClip selectBotton3;
    public AudioClip selectBotton4;
    public AudioClip WalkSound;
    public AudioClip DeathSound;
    public AudioClip VictorySound;
    public AudioClip ShotSound;
    public AudioClip ChipSound;
    public AudioClip DefeatSound;
    public AudioClip CookingSound;
    public AudioClip RealoadSound;
    public AudioClip JumpSound;

    // Start is called before the first frame update
    void Start()
    {
        managerBoton = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playselectBottonSound1()
    {
        managerBoton.PlayOneShot(selectBotton1, 2.0f);
    }

    public void playselectBottonSound2()
    {
        managerBoton.PlayOneShot(selectBotton2, 2.0f);
    }

    public void playselectBottonSound3()
    {
        managerBoton.PlayOneShot(selectBotton3);
    }

    public void playselectBottonSound4()
    {
        managerBoton.PlayOneShot(selectBotton4);
    }
    
    public void playWalkSound()
    {
        managerBoton.PlayOneShot();
    }
    
    public void playDeathSound()
    {
        managerBoton.PlayOneShot();
    }
    
    public void playVictorySound()
    {
        managerBoton.PlayOneShot();
    }
    
    public void playShotSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    
    public void playJumpSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    
    public void playRealoadSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    
    public void playChipSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    public void playCookingSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    
    public void playDefeatSound()
    {
        managerBoton.PlayOneShot(sonidoBoton4);
    }
    

}
