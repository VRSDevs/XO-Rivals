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
       
    }
    // Start is called before the first frame update
    void Start()
    {
       

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
