using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class PlatformMinigameController : MonoBehaviour
{ 

    // Gamemanager
    private GameManager _gameManager;

    // Camara para el movimiento horizontal
    [SerializeField]
    private Camera cam;

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

    // Start is called before the first frame update
    void Start()
    {
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
