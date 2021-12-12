using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ComidaControllerOff : MonoBehaviour
{

    // Boolean de prueba de cambio personaje
    public bool playerColor = true;

    public MatchAI thisMatch;

    public PlayerInfo localPlayer;

    // Sounds
    public SFXManagerComida sounds;
    public AudioClip ComiditasMusic;
    
    // Canvas final
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

    // Player
    [SerializeField]
    private Rigidbody2D player;
    [SerializeField]
    private Rigidbody2D playerORB;

    // Tipo jugador
    [SerializeField]
    private GameObject playerO;
    [SerializeField]
    private GameObject playerX;

    // Gamemanager
    private GameManager _gameManager;

    // Controles de movil
    [SerializeField]
    public GameObject leftButton;
    [SerializeField]
    public GameObject rightButton;

    // Cronometro
    public TextMeshProUGUI crono;
    private float time = 20;
    private bool finished = false;

    // variables para animaciones
    public bool win = false;
    public bool lost = false;

    // Minijuegos
    Generador generador;
    [SerializeField]
    private int orden = 1;
    [SerializeField]
    GameObject panAbajo;
    [SerializeField]
    GameObject panArriba;
    [SerializeField]
    GameObject queso;
    [SerializeField]
    GameObject carne;
    [SerializeField]
    GameObject lechuga;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = FindObjectOfType<MatchAI>();
        //localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        localPlayer = FindObjectOfType<PlayerInfo>();
    }   


    void Start()
    {
        crono.text = " " + time;

        

        generador = FindObjectOfType<Generador>();

        StartCoroutine(DefeatNumerator());

        // Valor a cambiar segun el color de ficha del jugador
        if (thisMatch.PlayerOName == localPlayer.Name)
        {
            playerO.SetActive(true);
            playerX.SetActive(false);
            
        } else
        {
            playerO.SetActive(false);
            playerX.SetActive(true);
        }

        if (!_gameManager.IsWebGLMobile)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
        }

        FindObjectOfType<AudioManager>().StopAllSongs();
        FindObjectOfType<AudioManager>().ChangeMusic(ComiditasMusic,"Tic-Tac-Toe");
    }

    // Update is called once per frame
    void Update()
    {

        if (time >= 0 && !finished)
        {
            time -= Time.deltaTime;
            crono.text = " " + time.ToString("f0");
        }

        if (time < 0)
        {
            // Aqui se manda a alberto la derrota
            stopGenerador();
            crono.SetText("0:00");
            panAbajo.SetActive(false);
            queso.SetActive(false);
            carne.SetActive(false);
            lechuga.SetActive(false);
            panArriba.SetActive(false);
            orden = 1;
            player.constraints = RigidbodyConstraints2D.FreezeAll;
            playerORB.constraints = RigidbodyConstraints2D.FreezeAll;
            lost = true;           
            
        }

        if (win)
        {
            panAbajo.SetActive(true);
            queso.SetActive(true);
            carne.SetActive(true);
            lechuga.SetActive(true);
            panArriba.SetActive(true);
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
        thisMatch.TurnMoment = 2;
        SceneManager.LoadScene("TicTac_AI");
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        thisMatch.TurnMoment = 2;
        SceneManager.LoadScene("TicTac_AI");
    }


    void startGenerador()
    {
        generador.stopSpawning = false;
    }

    void stopGenerador()
    {
        generador.stopSpawning = true;
    }

    public void recibirComida(int tipo)
    {
        switch (tipo)
        {
            // Recibimos un pan de abajo
            case 1:
                if (orden == 1)
                {
                    // Añadir al Hud
                    panAbajo.SetActive(true);
                    orden++;
                }           
                     else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;

            // Recibimos un queso
            case 2:
                if (orden == 2)
                {
                    // Añadir al Hud
                    queso.SetActive(true);
                    orden++;
                } else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;

            // Recibimos carne
            case 3:
                if (orden == 3)
                {
                    // Añadir al Hud
                    carne.SetActive(true);
                    orden++;
                }
                else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;

            // Recibimos lechuga
            case 4:
                if (orden == 4)
                {
                    // Añadir al Hud
                    lechuga.SetActive(true);
                    orden++;
                }
                else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;
            case 5:
                if (orden == 5)
                {
                    // Fin minijuego mandar bool a alberto
                    panArriba.SetActive(true);
                    finished = true;
                    win = true;
                    crono.SetText("0:00");
                    stopGenerador();
                    playerORB.constraints = RigidbodyConstraints2D.FreezeAll;
                    player.constraints = RigidbodyConstraints2D.FreezeAll;
                    // Aqui se manda a alberto la victoria
                    Invoke("VictoryCanvas", 3f);

                }
                else
                {
                    panAbajo.SetActive(false);
                    queso.SetActive(false);
                    carne.SetActive(false);
                    lechuga.SetActive(false);
                    panArriba.SetActive(false);
                    orden = 1;
                }
                break;

        }



    }

    IEnumerator DefeatNumerator()
    {
        yield return new WaitUntil(ReturnLost);

        DefeatCanvas();
    } 

    public bool ReturnLost()
    {
        return lost;
    }
}
