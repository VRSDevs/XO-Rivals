using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComidaController : MonoBehaviour
{
    
    

    Generador generador;

    [SerializeField]
    private int orden = 1;
    [SerializeField]
    GameObject panAbajo;
    [SerializeField]
    GameObject panArriba;
    [SerializeField]
    GameObject queso;
    [SerializeField]
    GameObject carne;
    [SerializeField]
    GameObject lechuga;

    // Start is called before the first frame update
    void Start()
    {
        generador = FindObjectOfType<Generador>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void startGenerador()
    {
        generador.stopSpawning = false;
    }

    void stopGenerador()
    {
        generador.stopSpawning = true;
    }

    public void recibirComida(int tipo)
    {

        switch (tipo)
        {
            // Recibimos un pan
            case 1:
                if (orden == 1)
                {
                    // Añadir al Hud
                    panAbajo.SetActive(true);
                    orden++;
                } else if (orden == 5)
                {
                    // Fin minijuego mandar bool a alberto
                    panArriba.SetActive(true);

                    //SceneManager.UnloadSceneAsync("Minijuego Comida");

                    // Aqui se manda a alberto
                    PlayerPrefs.SetInt("minigameWin", 1);



                } else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;

            // Recibimos un queso
            case 2:
                if (orden == 2)
                {
                    // Añadir al Hud
                    queso.SetActive(true);
                    orden++;
                } else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }


                break;

            // Recibimos carne
            case 3:
                if (orden == 3)
                {
                    // Añadir al Hud
                    carne.SetActive(true);
                    orden++;
                }
                else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }


                break;

            // Recibimos lechuga
            case 4:
                if (orden == 4)
                {
                    // Añadir al Hud
                    lechuga.SetActive(true);
                    orden++;
                }
                else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }


                break;

        }



    }
}
