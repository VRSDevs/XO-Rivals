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

    // Variables auxiliares a cambiarse
    bool jugadorX = false;
    int level = 1;


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

        if (jugadorX)
        {
            playerO.SetActive(false);

        }
        else
        {
            playerX.SetActive(false);
            flag1.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;
            flag2.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;
            flag3.gameObject.GetComponent<SpriteRenderer>().sprite = flagSpriteO;
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
