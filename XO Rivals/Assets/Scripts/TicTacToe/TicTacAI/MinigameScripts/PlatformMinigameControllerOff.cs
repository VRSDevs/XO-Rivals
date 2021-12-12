using UnityEngine;

public class PlatformMinigameControllerOff : MonoBehaviour
{ 

    // Gamemanager
    private GameManager _gameManager;

    public MatchAI thisMatch;

    public PlayerInfo localPlayer;

    // Controles de movil
    [SerializeField]
    public GameObject jumpButton;
    [SerializeField]
    public GameObject rightButton;
    
    // Jugadores
    [SerializeField]
    public GameObject playerX;
    [SerializeField]
    public GameObject playerO;

    // Niveles
    [SerializeField]
    public GameObject level1;
    [SerializeField]
    public GameObject level2;
    [SerializeField]
    public GameObject level3;

    // Banderas
    [SerializeField]
    private GameObject flag1;
    [SerializeField]
    private GameObject flag2;
    [SerializeField]
    private GameObject flag3;
    [SerializeField]
    private Sprite flagSpriteO;
    
    //Sounds
    public AudioClip PlatformMusic;

    // Variables auxiliares a cambiarse
    bool jugadorX = false;
    public int level = 3;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = FindObjectOfType<MatchAI>();
        //localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        localPlayer = FindObjectOfType<PlayerInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (level)
        {
            case 1:
                level1.SetActive(true);
                level2.SetActive(false);
                level3.SetActive(false);
                break;
            case 2:
                level1.SetActive(false);
                level2.SetActive(true);
                level3.SetActive(false);
                break;
            case 3:
                level1.SetActive(false);
                level2.SetActive(false);
                level3.SetActive(true);
                break;
        }

        if (thisMatch.PlayerOName == localPlayer.Name)
        {
            playerX.SetActive(false);
            flag1.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;
            flag2.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;
            flag3.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;       
        }
        else
        {
            playerO.SetActive(false);
        }

        if (!_gameManager.IsWebGLMobile)
        {
            jumpButton.SetActive(false);
            rightButton.SetActive(false);
        }
        
        FindObjectOfType<AudioManager>().StopAllSongs();
        FindObjectOfType<AudioManager>().ChangeMusic(PlatformMusic,"Tic-Tac-Toe");
    }
    
    public void playJumpSound()
    {
        FindObjectOfType<AudioManager>().Play("Jump");
    }
}
