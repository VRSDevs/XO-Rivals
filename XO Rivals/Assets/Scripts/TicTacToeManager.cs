using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    private NetworkController _networkController;

    [SerializeField] public TMP_Text PlayerCounter;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _networkController = FindObjectOfType<NetworkController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerCounter.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
}
