using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool activatedSounds = false;
    public float musicVolume;
    public float soundVolume;
    public AudioManager AudioManager;
    void Awake() {
        //GameObject.Find("GeneralVolume").GetComponent<Toggle>().isOn = activatedSounds;
    }
    public void SetMusicVolume(float volume) {
        AudioManager.SetMusicVolume(volume, soundTypes.M);
    }

    public void SetSoundVolume(float volume) {
        AudioManager.SetSoundVolume(volume, soundTypes.S);
    }

    public void MuteSounds(bool muteSound) {
        AudioManager.MuteSounds(muteSound);
    }

}
