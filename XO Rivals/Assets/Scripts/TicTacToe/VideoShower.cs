using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoShower : MonoBehaviour
{

    [SerializeField]
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Ghost_Hunter.mp4");
    }

    private void Start()
    {
        videoPlayer.Play();
    }
}
