using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cronometro : MonoBehaviour
{
    // Start is called before the first frame update

    public float time = 8;
    public int tiempoDesaparecido = 4;
    public bool activo = false;
    public Text textoCrono;
    public Text textoExplicativo;
    public float timeEnemy=-1;
    public float maxEnemy = 2f;

    void Start()
    {
        
        
    }

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
                textoCrono.text = "-" + ":" + "--";
            }

        }

    }

    public void activarCrono()
    {
        activo = true;
        textoExplicativo.text = "Suelta el botón cuando el cronometro llegue a 0:00";
        timeEnemy = UnityEngine.Random.Range(0f, maxEnemy);
    }
    public void finCrono()
    {
        activo = false;

        //SEPARAMOS SEGUNDOS Y DECIMAS DEL TIEMPO RESULTADO
        int segundos = (int)Math.Truncate(time);
        int decimas = (int)((time - segundos) * 100); //Sacamos dos decimas
        decimas = Math.Abs(decimas);//el signo negativo solo tiene que estar en los segundos

        textoCrono.text = segundos + ":" + decimas;

  //TiempoEnemigo
        int segundosEnemy = (int)Math.Truncate(timeEnemy);
        int decimasEnemy = (int)((timeEnemy - segundosEnemy) * 100); //Sacamos dos decimas


        //VICTORIA

        if (time<timeEnemy && time >=0)
        {

            StartCoroutine("EsperarV");

            textoExplicativo.text = "ESTE ES TU RESULTADO!\nVICTORIA CONTRA "+ segundosEnemy + ":" + decimasEnemy ;

        }//DERROTA
        else
        {
            StartCoroutine("EsperarD");
            textoExplicativo.text = "ESTE ES TU RESULTADO!\nDERROTA CONTRA " + segundosEnemy + ":" + decimasEnemy ;

        }
      

    }

    IEnumerator EsperarV()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("PistoleroVictoria");
    }
    IEnumerator EsperarD()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Pistolero Derrota");
    }



}
