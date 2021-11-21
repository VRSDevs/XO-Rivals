using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFantasmasController : MonoBehaviour
{

    // Controles de movil
    [SerializeField]
    public GameObject leftButton;
    [SerializeField]
    public GameObject rightButton;
    [SerializeField]
    public GameObject upButton;
    [SerializeField]
    public GameObject downButton;

    // Gamemanager
    private GameManager _gameManager;

    //Musica
    public AudioClip MusicaBosque;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        if (!_gameManager.IsWebGLMobile)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            upButton.SetActive(false);
            downButton.SetActive(false);
        }
        
        FindObjectOfType<AudioManager>().StopAllSongs();
        FindObjectOfType<AudioManager>().ChangeMusic(MusicaBosque,"Tic-Tac-Toe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
