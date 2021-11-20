using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicImageAnim : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;
    [SerializeField]
    private Sprite sprite3;

    [SerializeField]
    private Image image;

    // Update is called once per frame
    void Update()
    {
        RefreshAnimMusic();
    }

    void RefreshAnimMusic()
    {
        if (slider.value < 0.25){
            image.sprite = sprite1;
        } else if (slider.value >= 0.25 && slider.value < 0.75)
        {
            image.sprite = sprite2;
        } else
        {
            image.sprite = sprite3;
        }

    }

}
