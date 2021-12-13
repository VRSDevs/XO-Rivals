using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ScriptHuella : MonoBehaviour
{

    public GameObject sigHuella;

    public GameObject renderX;
    public GameObject renderO;

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
            renderX.SetActive(false);

        }
        else
        {
            renderO.SetActive(false);

        }

        StartCoroutine(deleteHuella());


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator deleteHuella()
    {
        yield return new WaitForSeconds(15f);

        Destroy(this.gameObject);

    }

}
