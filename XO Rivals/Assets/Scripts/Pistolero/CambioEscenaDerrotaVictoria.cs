using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaDerrotaVictoria : MonoBehaviour
{
    // Start is called before the first frame update
    public bool win;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void cambiarEscena()
    {

        if (win)
        {
            SceneManager.UnloadSceneAsync("Pistolero");
            PlayerPrefs.SetInt("minigameWin", 1);

        }
        else
        {
            SceneManager.UnloadSceneAsync("Pistolero");
            PlayerPrefs.SetInt("minigameWin", 0);

        }


    }



}