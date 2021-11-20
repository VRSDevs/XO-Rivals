using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManagerFantasma : MonoBehaviour
{
    private AudioSource managerFantasmas;
    public AudioClip CookingSound;
    public AudioClip DeathSound;
    public AudioClip VictorySound;
    public AudioClip DefeatSound;
    public AudioClip WalkSound;
    
    
    void Start()
    {
        managerFantasmas = GetComponent<AudioSource>();
    }
    public void playDeathSound()
    {
        managerFantasmas.PlayOneShot(DeathSound);
    }
    public void playVictorySound()
    {
        managerFantasmas.PlayOneShot(VictorySound);
    }
    public void playDefeatSound()
    {
        managerFantasmas.PlayOneShot(DefeatSound);
    }
    public void playWalkSound()
    {
        managerFantasmas.PlayOneShot(WalkSound);
    }

}
