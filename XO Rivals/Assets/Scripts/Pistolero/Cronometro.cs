using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour
{
    // Start is called before the first frame update

    public float time = 8;
    public int tiempoDesaparecido = 4;
    public bool activo = false;
    public Text textoCrono;
    public Text textoExplicativo;

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

    }
    public void finCrono()
    {
        activo = false;

        //SEPARAMOS SEGUNDOS Y DECIMAS DEL TIEMPO RESULTADO
        int segundos = (int)Math.Truncate(time);
        int decimas = (int)((time - segundos) * 100); //Sacamos dos decimas
        decimas = Math.Abs(decimas);//el signo negativo solo tiene que estar en los segundos

        textoCrono.text = segundos + ":" + decimas;
        textoExplicativo.text = "ESTE ES TU RESULTADO!";

    }


}
