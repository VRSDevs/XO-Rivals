using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TicTacToeControllerAI : MonoBehaviour
{

    public AudioClip Tic_Tac_toe_Music;
    
    void Start()
    {
        //FindObjectOfType<AudioManager>().StopAllSongs();
        //FindObjectOfType<AudioManager>().ChangeMusic(Tic_Tac_toe_Music,"Main_menu");
    }

    public void PutChip()
    {
        //FindObjectOfType<AudioManager>().Play("Chip");
    }
    
    public void SelectButton()
    { 
        //FindObjectOfType<AudioManager>().Play("SelecctionButton1");
    }

    public void Surrender(){
        SceneManager.LoadScene("MainMenu");
    }
}
