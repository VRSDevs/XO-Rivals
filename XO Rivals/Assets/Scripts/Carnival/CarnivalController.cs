using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CarnivalController : MonoBehaviour
{

    public bool win = false;
    public bool lost = false;

    [SerializeField]
    private Transform bottom;
    [SerializeField]
    private Transform top;
    [SerializeField]
    private Transform bottomWin;
    [SerializeField]
    private Transform topWin;
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private GameObject playerO;
    [SerializeField]
    private GameObject playerX;
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

    public bool goingUp = true;

    private float speed = 500;

    // Gamemanager
    private GameManager _gameManager;

    public Match thisMatch;

    public PlayerInfo localPlayer;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = _gameManager.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // Recoger de quien es el turno y activar los personajes necesarios
        if (thisMatch.PlayerOName == localPlayer.Name)
        {
            playerX.SetActive(false);
        }
        else
        {
            playerO.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BarMovement();
        if (indicator.transform.position.y > top.position.y - 10)
        {
            goingUp = false;
        }

        if (indicator.transform.position.y < bottom.position.y + 10)
        {
            goingUp = true;
        }
    }

    private void BarMovement()
    {
        if (goingUp)
        {      
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(indicator.transform.position, top.position, step);
            indicator.transform.position = aux;
        }
        else
        {
            float step = speed * Time.deltaTime;
            Vector2 aux = Vector2.MoveTowards(indicator.transform.position, bottom.position, step);
            indicator.transform.position = aux;
        }
    }

    public void PressedButon()
    {
        speed = 0;
        CheckVictory();
    }

    public void CheckVictory()
    {
        if (indicator.transform.position.y < topWin.position.y && indicator.transform.position.y > bottomWin.position.y)
        {
            win = true;
            VictoryCanvas();
        }
        else
        {
            lost = true;
            DefeatCanvas();
        }
    }

    public void DefeatCanvas()
    {
        defeat.SetActive(true);
        Invoke("Defeat", 3f);
        FindObjectOfType<AudioManager>().Play("Defeat");

    }

    public void VictoryCanvas()
    {
        victory.SetActive(true);
        Invoke("Victory", 3f);
        FindObjectOfType<AudioManager>().Play("Victory");

    }

    public void Defeat()
    {
        PlayerPrefs.SetInt("minigameWin", 0);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        SceneManager.LoadScene("TicTacToe_Server");
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        FindObjectOfType<GameManager>().PlayerMatches[Photon.Pun.PhotonNetwork.CurrentRoom.Name].TurnMoment = 2;
        SceneManager.LoadScene("TicTacToe_Server");
    }
}
