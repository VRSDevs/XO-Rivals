using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoShowerPlatform : MonoBehaviour
{
    [SerializeField]
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Gravitational_Jumps.mp4");
    }

    private void Start()
    {
        videoPlayer.Play();
    }
}
