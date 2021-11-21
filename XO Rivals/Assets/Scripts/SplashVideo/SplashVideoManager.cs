using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashVideoManager : MonoBehaviour
{

    [SerializeField]
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.mp4");
    }

    void Start()
    {
        videoPlayer.Play();
        Invoke("GoToNextScene", 4.5f);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
