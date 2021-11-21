using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cronometro : MonoBehaviour
{
    // Start is called before the first frame update

    public bool win = false;
    public bool lost = false;

    public float time = 8;
    public int tiempoDesaparecido = 7;
    public bool activo = false;
    public Text textoCrono;
    public Text textoExplicativo;
    public float timeEnemy=-1;
    public float maxEnemy = 2f;
    public CambioEscenaDerrotaVictoria cambioEscena;
    public GameObject pergamino;

    // Update is called once per frame
    void Update()
    {

        if (activo)
        {

            time -= Time.deltaTime;
            //text.text = time.ToString();

            //AHORA SEPARAMOS SEGUNDOS Y DECIMAS
            int segundos = (int)Math.Truncate(time);
            int decimas = (int)((time - segundos)*100); //Sacamos dos decimas
            decimas = Math.Abs(decimas);//el signo negativo solo tiene que estar en los segundos

            textoCrono.text = segundos+":"+decimas;

            //LOS ULTIMOS TRES SEGUNDOS DESAPARECE EL TIEMPO
            if (segundos<tiempoDesaparecido )
            {
                float auxAlfa = textoCrono.color.a -0.001f;
                //textoCrono.color = new Color(textoCrono.color.r, textoCrono.color.g, textoCrono.color.b, auxAlfa);
                textoCrono.text = "--:--";
            }

        }

    }

    public void activarCrono()
    {
        activo = true;
        //textoExplicativo.text = "Release when the timer reaches 0:00";
        timeEnemy = UnityEngine.Random.Range(0f, maxEnemy);
    }
    public void finCrono()
    {
        activo = false;
        textoCrono.color = new Color(textoCrono.color.r, textoCrono.color.g, textoCrono.color.b, 1);

        //SEPARAMOS SEGUNDOS Y DECIMAS DEL TIEMPO RESULTADO
        int segundos = (int)Math.Truncate(time);
        int decimas = (int)((time - segundos) * 100); //Sacamos dos decimas

        decimas = Math.Abs(decimas);//el signo negativo solo tiene que estar en los segundos



        if (time < 0 && time > -1) //el signo negativo lo ponemos nosotros debido a que en los segundos no est� (en las decimas si)
        {


            if (decimas < 10) //si son mas peque�s que 10 no sale el 0 de antes
            {
                textoCrono.text = "-" + segundos + ":" + "0" +decimas;
            }
            else
            {
                textoCrono.text = "-" + segundos + ":" + decimas;
            }

           
        }
        else
        {

            if (decimas < 10) //si son mas peque�s que 10 no sale el 0 de antes
            {
                textoCrono.text = segundos + ":" + "0" + decimas;
            }
            else
            {
                textoCrono.text = segundos + ":" + decimas;
            }
            
        }
       

  //TiempoEnemigo
        int segundosEnemy = (int)Math.Truncate(timeEnemy);
        int decimasEnemy = (int)((timeEnemy - segundosEnemy) * 100); //Sacamos dos decimas


        //VICTORIA

        if (time<timeEnemy && time >=0)
        {

            //StartCoroutine("EsperarV");
            cambioEscena.win = true;

            win = true;

            if (decimasEnemy < 10) //si son mas peque�s que 10 no sale el 0 de antes
            {
                pergamino.SetActive(true);
                textoExplicativo.text = "VICTORY AGAINST " + segundosEnemy + ":" + "0"+ decimasEnemy + "\n \n Your time:";
            }
            else
            {
                pergamino.SetActive(true);
                textoExplicativo.text = "VICTORY AGAINST " + segundosEnemy + ":" + decimasEnemy + "\n \n Your time:";
            }

           

        }//DERROTA
        else
        {
            lost = true;

            cambioEscena.win = false;
            //StartCoroutine("EsperarD");
            if (decimasEnemy < 10) //si son mas peque�s que 10 no sale el 0 de antes
            {
                pergamino.SetActive(true);
                textoExplicativo.text = "LOST AGAINST " + segundosEnemy + ":" + "0"+decimasEnemy + "\n \n Your time:";
            }
            else
            {
                pergamino.SetActive(true);
                textoExplicativo.text = "LOST AGAINST " + segundosEnemy + ":" + decimasEnemy + "\n \n Your time:";
            }

           

        }
    }

    IEnumerator EsperarV()
    {
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("minigameWin", 1);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        SceneManager.LoadScene("TicTacToe_Server");
        //SceneManager.LoadScene("PistoleroVictoria", LoadSceneMode.Additive);
    }
    IEnumerator EsperarD()
    {
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("minigameWin", 0);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        SceneManager.LoadScene("TicTacToe_Server");
        //SceneManager.LoadScene("Pistolero Derrota", LoadSceneMode.Additive);
    }



}
