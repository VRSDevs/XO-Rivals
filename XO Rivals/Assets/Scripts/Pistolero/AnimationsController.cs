using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{

    // GameObjects
    [SerializeField]
    private GameObject playerO;
    [SerializeField]
    private GameObject playerX;
    [SerializeField]
    private GameObject iaO;
    [SerializeField]
    private GameObject iaX;

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


    // Start is called before the first frame update
    void Start()
    {
        crono = FindObjectOfType<Cronometro>();

        // Recoger de quien es el turno y activar los personajes necesarios
        if (prueba)
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

        } 
        // Activar animación jugador
        else if (crono.win == true)
        {
            AnimationPlayer0();
            AnimationPlayerX();
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
