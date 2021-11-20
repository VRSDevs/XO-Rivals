using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PerderyGanar : MonoBehaviour
{

    // Sounds
    public SFXManagerFantasma sounds;

    
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

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

            Invoke("DefeatCanvas", 3f);

        }


    }

    IEnumerator contarSegundosParaGanar()
    {

        yield return new WaitForSeconds(1);

        if (seconds == 20)//SI AGUANTAS 20 SEGUNDOS GANAS
        {          
            Invoke("VictoryCanvas", 2f);
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

    public void DefeatCanvas()
    {
        defeat.SetActive(true);
        Invoke("Defeat", 3f);
        sounds.playDefeatSound();
    }

    public void VictoryCanvas()
    {
        victory.SetActive(true);
        Invoke("Victory", 3f);
        sounds.playVictorySound();

    }

    public void Defeat()
    {
        PlayerPrefs.SetInt("minigameWin", 0);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        //SceneManager.UnloadSceneAsync("MinijuegoFantasmas");
        SceneManager.LoadScene("TicTacToe_Server");
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        //SceneManager.UnloadSceneAsync("MinijuegoFantasmas");
        SceneManager.LoadScene("TicTacToe_Server");
    }


}
