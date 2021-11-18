using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PerderyGanar : MonoBehaviour
{
    public int seconds =3;
    public Text textoSegundos;
    public Text textoCuentaAtras;
    public List<GameObject> enemigos;

    // Start is called before the first frame update
    void Start()
    {
        textoCuentaAtras.text = "" + seconds;
        StartCoroutine(cuentaAtras()); //PRIMERO HACEMOS LA CUENTA ATRAS
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)//SI CHOCAS CON FANTASMA PIERDES
    {
        if (collision.gameObject.name == "Enemy3D")
        {

            Debug.Log("Perdido!");
            PlayerPrefs.SetInt("minigameWin", 0);
            SceneManager.UnloadSceneAsync("Fantasmas3D");


        }


    }

    IEnumerator contarSegundosParaGanar()
    {

        yield return new WaitForSeconds(1);

        if (seconds == 20)//SI AGUANTAS 20 SEGUNDOS GANAS
        {
            textoSegundos.color = Color.green;
            textoCuentaAtras.text = "WIN";
            StartCoroutine(victoria());

        }
        else
        {
            seconds++;
            textoSegundos.text = "" + seconds;
            StartCoroutine(contarSegundosParaGanar());
        }



    }
    IEnumerator cuentaAtras()
    {
        yield return new WaitForSeconds(1);

        if (seconds ==1)
        {
            seconds = 0;
            textoCuentaAtras.text = "";
            textoSegundos.text = "" + seconds;
            for (int i = 0; i < enemigos.Count; i++)
            {
                enemigos[i].SetActive(true);
            }
            StartCoroutine(contarSegundosParaGanar()); //PRIMERO HACEMOS LA CUENTA ATRAS

        }
        else
        {
            seconds--;
            textoCuentaAtras.text = ""+seconds;
            StartCoroutine(cuentaAtras()); 
        }


    }


    IEnumerator victoria()
    {
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("minigameWin", 1);
        SceneManager.UnloadSceneAsync("Fantasmas3D");
    }


    }
