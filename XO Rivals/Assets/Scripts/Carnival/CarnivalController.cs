using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarnivalController : MonoBehaviour
{
    // Variables de victoria y derrota
    public bool win = false;
    public bool lost = false;

    // Control del personaje
    public bool playinWithO = false;

    // Gameobjects y transforms necesarios
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
    [SerializeField]
    private Sprite playerXWin;
    [SerializeField]
    private Sprite playerXLost;
    [SerializeField]
    private Sprite playerOWin;
    [SerializeField]
    private Sprite playerOLost;
    [SerializeField]
    private Animator glassAnim;

    [SerializeField]
    private Button boton;

    // Control del indicador
    public bool goingUp = true;
    private float speed = 500;

    // Gamemanager
    private GameManager _gameManager;

    public Match thisMatch;

    public PlayerInfo localPlayer;
    
    public AudioClip feriaMusic;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = _gameManager.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

    }

    // Start is called before the first frame update
    void Start()
    {

        Invoke("ActivateButton", 1f);
        speed = Random.Range(700,850);
        // Recoger de quien es el turno y activar los personajes necesarios
        if (thisMatch.PlayerOName == localPlayer.Name)
        {
            playerX.SetActive(false);
            playinWithO = true;
        }
        else
        {
            playerO.SetActive(false);
        }
        FindObjectOfType<AudioManager>().StopAllSongs();
        FindObjectOfType<AudioManager>().ChangeMusic(feriaMusic,"Tic-Tac-Toe");
    }

    private void FixedUpdate()
    {
        if (!win && !lost)
        {
            BarMovement();
        }      
    }

    // Update is called once per frame
    void Update()
    {
 
        if (indicator.transform.position.y > top.position.y - 10)
        {
            goingUp = false;
        }

        if (indicator.transform.position.y < bottom.position.y + 10)
        {
            goingUp = true;
        }
    }

    // Movimiento del indicador
    private void BarMovement()
    {
        speed += 3f;
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

    // Pulsar botón
    public void PressedButon()
    {
        glassAnim.SetTrigger("pressed");
        speed = 0;
        CheckVictory();
    }

    // Comprobar victoria o derrota
    public void CheckVictory()
    {
        if (indicator.transform.position.y < topWin.position.y && indicator.transform.position.y > bottomWin.position.y)
        {
            win = true;
            Invoke("VictoryCanvas", 1f);
            if (playinWithO)
            {
                playerO.GetComponent<Image>().sprite = playerOWin;

            } 
            else
            {
                playerX.GetComponent<Image>().sprite = playerXWin;
            }
        }
        else
        {
            lost = true;
            Invoke("DefeatCanvas", 1f);
            if (playinWithO)
            {
                playerO.GetComponent<Image>().sprite = playerOLost;

            }
            else
            {
                playerX.GetComponent<Image>().sprite = playerXLost;
            }
        }
    }

    // Animaciones de victoria y derrota
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

    private void ActivateButton()
    {
        boton.interactable = true;
    }
}
