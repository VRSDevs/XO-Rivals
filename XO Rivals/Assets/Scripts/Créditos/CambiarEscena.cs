using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambiarEscena(string escenaC)
    {
        SceneManager.LoadScene(escenaC);
    }

    public void abrirLink(string link)
    {

        Application.OpenURL(link);
    }


}
