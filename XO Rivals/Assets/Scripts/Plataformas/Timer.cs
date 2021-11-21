using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{

    public TextMeshProUGUI crono;
    public Player ScriptPlayer;
    public bool lost = false;

    private float time = 30f;
    // Start is called before the first frame update
    void Start()
    {
        crono.text = " " + time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0 && ScriptPlayer.Victory == false)
        {
            time -= Time.deltaTime;
            crono.text = " " + time.ToString("f0"); 
        }
        
        if (time < 0)
        {
            if (ScriptPlayer.Victory == false)
            {
                ScriptPlayer.textValue = " Game Over";
                lost = true;
                ScriptPlayer.OnDisable();

                FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
                PlayerPrefs.SetInt("minigameWin", 0);
                SceneManager.LoadScene("TicTacToe_Server");
            }

        }
    }  
}
