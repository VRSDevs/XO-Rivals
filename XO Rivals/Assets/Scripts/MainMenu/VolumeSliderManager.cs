using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManager : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public SFXManager manager;
    public float sliderValueSfx;
    public float sliderValueMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        manager.volumenMusica = musicSlider.value;
        manager.volumenSFX = sfxSlider.value;
    }


    public void cambiarSliderSFX(float valor)
    {
        sliderValueSfx = valor;
        manager.volumenSFX = sfxSlider.value;
    }

    public void cambiarSliderMusica(float valor)
    {
        sliderValueMusic = valor;
        manager.volumenMusica = musicSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
