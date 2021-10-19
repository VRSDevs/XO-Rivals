using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlEscenas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void arte()
    {
        SceneManager.LoadScene("CreditosArte");
    }

    public void programacion()
    {

        SceneManager.LoadScene("CreditosProgramacion");
    }
    public void sonido()
    {
        SceneManager.LoadScene("CreditosSonido");
    }

    public void salir()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }


}
