using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationsController : MonoBehaviour
{

    public Match thisMatch;

    public PlayerInfo localPlayer;

    // GameObjects
    [SerializeField]
    private GameObject playerO;
    [SerializeField]
    private GameObject playerX;
    [SerializeField]
    private GameObject iaO;
    [SerializeField]
    private GameObject iaX;
    [SerializeField]
    private GameObject bangIA;
    [SerializeField]
    private GameObject bangPl;

    // Animators
    [SerializeField]
    private Animator animPlayO;
    [SerializeField]
    private Animator animPlayX;
    [SerializeField]
    private Animator animIAO;
    [SerializeField]
    private Animator animIAX;

    // Cronometro
    private Cronometro crono;

    // Gamemanager
    private GameManager _gameManager;

    public bool prueba = true;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        thisMatch = _gameManager.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

    }

    // Start is called before the first frame update
    void Start()
    {
        crono = FindObjectOfType<Cronometro>();

        // Recoger de quien es el turno y activar los personajes necesarios
        if (thisMatch.PlayerOName == localPlayer.Name)
        {
            playerX.SetActive(false);
            iaO.SetActive(false);
        } 
        else
        {
            playerO.SetActive(false);
            iaX.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!crono.lost && !crono.win) return;

        // Activar animación IA
        if (crono.lost == true)
        {
            AnimationIA0();
            AnimationIAX();
            bangIA.SetActive(true);

        } 
        // Activar animación jugador
        else if (crono.win == true)
        {
            AnimationPlayer0();
            AnimationPlayerX();
            bangPl.SetActive(true);
        }
        
    }

    public void AnimationPlayer0()
    {
        animPlayO.SetBool("Disparo", true);
    }

    public void AnimationPlayerX()
    {
        animPlayX.SetBool("Disparo", true);
    }
    public void AnimationIA0()
    {
        animIAO.SetBool("Disparo", true);
    }
    public void AnimationIAX()
    {
        animIAX.SetBool("Disparo", true);
    }

}
