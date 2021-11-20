using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager_Pistolero : MonoBehaviour
{
    private AudioSource managerPistolero;

    public AudioClip ReloadSound;
    public AudioClip ShotSound;

    public AudioClip VictorySound;
    public AudioClip DefeatSound;

    // Start is called before the first frame update
    void Start()
    {
        managerPistolero = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void playReloadSound()
    {
        managerPistolero.PlayOneShot(ReloadSound);
    }
    public void playShotSound()
    {
        managerPistolero.PlayOneShot(ShotSound);
    }
    
    public void playVictorySound()
    {
        managerPistolero.PlayOneShot(VictorySound);
    }
    public void playDefeatSound()
    {
        managerPistolero.PlayOneShot(DefeatSound);
    }
    
    
    
}
