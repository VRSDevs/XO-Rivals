using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMusic : MonoBehaviour
{
    [SerializeField]
    private Slider sliderMusic;
    [SerializeField]
    private Slider sliderSound;

    [SerializeField]
    private Animator anim;


    // Update is called once per frame
    void Update()
    {
        RefreshAnimationSpeed();
    }

    void RefreshAnimationSpeed()
    {
        float average = 0;
        average = (sliderMusic.value + sliderSound.value) / 2;
        average = Mathf.Clamp(average, 0, 1);
        anim.SetFloat("average", average);
    }
}
