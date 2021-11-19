using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaDerrotaVictoria : MonoBehaviour
{

    public SFXManager_Pistolero sounds;
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

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
            Invoke("VictoryCanvas", 3f);
        }
        else
        {
            Invoke("DefeatCanvas", 3f);
        }


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
        //SceneManager.UnloadSceneAsync("Pistolero");
        SceneManager.LoadScene("TicTacToe_Server");
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        //SceneManager.UnloadSceneAsync("Pistolero");
        SceneManager.LoadScene("TicTacToe_Server");
    }

}
