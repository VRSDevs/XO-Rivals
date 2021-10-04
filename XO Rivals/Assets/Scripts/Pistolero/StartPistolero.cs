using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPistolero : MonoBehaviour
{
   public TextMesh text;
    public TextMesh boton;//HAY QUE CAMBIAR POR BOTON DE VERDAD


    // Start is called before the first frame update
    void Start()
    {
 
        StartCoroutine("Esperar");


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator Esperar()
    {
        yield return new WaitForSeconds(3);
        text.text = "El jugador que más \n se aproxime será \n el ganador...";//cambiamos el texto explicativo despues de 3 segundos
        StartCoroutine("Esperar2");
    }


    IEnumerator Esperar2()
    {
        yield return new WaitForSeconds(3);
        text.text = "Pulsa el botón para comenzar";//cambiamos el texto explicativo despues de 3 segundos
        boton.gameObject.SetActive(true);//Aparece elboton
    }




}
