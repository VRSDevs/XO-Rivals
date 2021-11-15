using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class PlatformMinigameController : MonoBehaviour
{ 

    // Gamemanager
    private GameManager _gameManager;

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

    // Bandera
    [SerializeField]
    private GameObject flag;
    [SerializeField]
    private Sprite flagO;

    bool jugadorX = false;


    // Start is called before the first frame update
    void Start()
    {
        if (jugadorX)
        {
            playerO.SetActive(false);

        }
        else
        {
            playerX.SetActive(false);
            flag.gameObject.GetComponent<SpriteRenderer>().sprite = flagO;
        }
        _gameManager = FindObjectOfType<GameManager>();

        if (!_gameManager.IsWebGLMobile)
        {
            jumpButton.SetActive(false);
            rightButton.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
