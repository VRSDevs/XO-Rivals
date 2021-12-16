using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPistolero : MonoBehaviour
{
    public Text text; //Texto explicativo
    public Button boton;//Boton de disparo

    public AudioClip GunmanMusic;

    
    // Start is called before the first frame update
    void Start()
    {
 
        StartCoroutine("Esperar"); //esperamos 3 segundos para que el jugador lea el texto

        FindObjectOfType<AudioManager>().StopAllSongs();
        FindObjectOfType<AudioManager>().ChangeMusic(GunmanMusic,"Tic-Tac-Toe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playReloadSound()
    {
        FindObjectOfType<AudioManager>().Play("Reload");
    }
    public void playShotSound()
    {
        FindObjectOfType<AudioManager>().Play("Shot");
    }


    IEnumerator Esperar()
    {
        yield return new WaitForSeconds(1);
        //text.text = "The closer you release \n the higher chances to win \n good luck...";//cambiamos el texto explicativo despues de 3 segundos
        StartCoroutine("Esperar2"); //volvemos a esperar 3 segundos para que el jugador lea el texto
    }


    IEnumerator Esperar2()
    {
        yield return new WaitForSeconds(2);
        //text.text = "Press the button to start\n(Hold it)";//cambiamos el texto explicativo despues de 3 segundos
        boton.gameObject.SetActive(true);//Aparece elboton
    }




}
