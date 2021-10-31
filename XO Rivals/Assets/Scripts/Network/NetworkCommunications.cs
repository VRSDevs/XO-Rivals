using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCommunications : MonoBehaviourPun
{
    /// <summary>
    /// 
    /// </summary>
    public void SendPackage()
    {
        Package packageToSend = FindObjectOfType<ButtonsScript>().gameState.ToPackage(FindObjectOfType<ButtonsScript>().localPlayer);
        
        Debug.Log("Vamos a mandar: " + packageToSend);        
        
        photonView.RPC("RPC_UpdateTurn", RpcTarget.All, packageToSend);
    }
    
    #region RPCMethods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="recievedPackage"></param>
    [PunRPC]
    public void RPC_UpdateTurn(Package recievedPackage)
    {
        Debug.Log("RPC recibido");
        
        FindObjectOfType<GameManager>().MatchId = recievedPackage.MatchId;
        FindObjectOfType<GameManager>().OwnerId = recievedPackage.OwnerId;
        if (FindObjectOfType<PlayerInfo>().Name == FindObjectOfType<GameManager>().PlayerInfoO.Name)
        {
            FindObjectOfType<GameManager>().PlayerInfoO = recievedPackage.Player;
        }
        else
        {
            FindObjectOfType<GameManager>().PlayerInfoX = recievedPackage.Player;
        }
        FindObjectOfType<GameManager>().WhosTurn = recievedPackage.WhosTurn;
        FindObjectOfType<GameManager>().NumFilled = recievedPackage.NumFilled;
        FindObjectOfType<GameManager>().FilledPositions = recievedPackage.FilledPositions;
        FindObjectOfType<GameManager>().Chips = recievedPackage.Chips;
        FindObjectOfType<GameManager>().MiniGameChosen = recievedPackage.MinigameChosen;

        FindObjectOfType<GameManager>().turnMoment = 0;

        FindObjectOfType<ButtonsScript>().UpdateTurn();
    }

    #endregion
}
