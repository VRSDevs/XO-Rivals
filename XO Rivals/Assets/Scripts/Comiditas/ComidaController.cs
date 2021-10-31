using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ComidaController : MonoBehaviour
{

    // Gamemanager
    private GameManager _gameManager;

    // Controles de movil
    [SerializeField]
    public GameObject leftButton;
    [SerializeField]
    public GameObject rightButton;

    // Cronometro
    public TextMeshProUGUI crono;
    private float time = 20;
    private bool finished = false;

    // Minijuegos
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
        crono.text = " " + time;

        _gameManager = FindObjectOfType<GameManager>();

        generador = FindObjectOfType<Generador>();


        if (!_gameManager.IsWebGLMobile)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0 && !finished)
        {
            time -= Time.deltaTime;
            crono.text = " " + time.ToString("f0");
        }

        if (time < 0)
        {
            // Aqui se manda a alberto la derrota
            stopGenerador();
            crono.SetText("Has perdido");
            Invoke("EndSceneLost", 5f);         
        }
    }

    void EndSceneLost()
    {
        PlayerPrefs.SetInt("minigameWin", 0);
        SceneManager.UnloadSceneAsync("MinijuegoComida");
    }

    void EndSceneWin()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        SceneManager.UnloadSceneAsync("MinijuegoComida");
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
                    finished = true;
                    crono.SetText("Has ganado");
                    stopGenerador();
                    // Aqui se manda a alberto la victoria
                    Invoke("EndSceneWin", 5f);                

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
