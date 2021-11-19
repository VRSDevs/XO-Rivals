using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaDerrotaVictoria : MonoBehaviour
{
    // Start is called before the first frame update
    public bool win;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void cambiarEscena()
    {

        if (win)
        {
            FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
            PlayerPrefs.SetInt("minigameWin", 1);
            SceneManager.LoadScene("TicTacToe_Server");
        }
        else
        {            
            FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
            PlayerPrefs.SetInt("minigameWin", 0);
            SceneManager.LoadScene("TicTacToe_Server");
        }


    }



}
