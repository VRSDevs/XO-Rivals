using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class SplashVideoManager : MonoBehaviour
{

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private Button but;

    private void Awake()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.mp4");
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        Invoke("GoToNextScene", 6f);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
