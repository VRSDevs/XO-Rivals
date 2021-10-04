using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cronometro : MonoBehaviour
{
    // Start is called before the first frame update

    public float time = 5;
    public bool activo = false;
    public TextMesh text;

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

            text.text = segundos+":"+decimas;

        }

    }
}
