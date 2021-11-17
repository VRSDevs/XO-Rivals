using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashVideoManager : MonoBehaviour
{
    void Start()
    {
        Invoke("GoToNextScene", 4.5f);
    }


    void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
