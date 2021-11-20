using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManagerComida : MonoBehaviour
{
    private AudioSource managerComidas;
    public AudioClip CookingSound;
    public AudioClip DeathSound;
    public AudioClip VictorySound;
    public AudioClip DefeatSound;
    public AudioClip WalkSound;
    
    
    void Start()
    {
        managerComidas = GetComponent<AudioSource>();
    }
    
    
    
    public void playDeathSound()
    {
        managerComidas.PlayOneShot(DeathSound);
    }
    public void playVictorySound()
    {
        managerComidas.PlayOneShot(VictorySound);
    }
    public void playDefeatSound()
    {
        managerComidas.PlayOneShot(DefeatSound);
    }
    public void playWalkSound()
    {
        managerComidas.PlayOneShot(WalkSound);
    }
    
    public void playCookingSound()
    {
        managerComidas.PlayOneShot(CookingSound);
    }
}
