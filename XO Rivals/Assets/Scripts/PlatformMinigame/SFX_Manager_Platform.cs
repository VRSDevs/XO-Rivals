using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager_Platform : MonoBehaviour
{
    private AudioSource managerPlatform;

    public AudioClip JumpSound;
    public AudioClip WalkSound;

    public AudioClip VictorySound;
    public AudioClip DefeatSound;
    public AudioClip DeathSound;

    // Start is called before the first frame update
    void Start()
    {
        managerPlatform = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void playJumpSound()
    {
        managerPlatform.PlayOneShot(JumpSound);
    }
    
    public void playDeathSound()
    {
        managerPlatform.PlayOneShot(DeathSound);
    }
    public void playWalkSound()
    {
        managerPlatform.PlayOneShot(WalkSound);
    }
    
    public void playVictorySound()
    {
        managerPlatform.PlayOneShot(VictorySound);
    }
    public void playDefeatSound()
    {
        managerPlatform.PlayOneShot(DefeatSound);
    }



}
