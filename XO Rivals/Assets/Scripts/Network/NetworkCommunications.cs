using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCommunications : MonoBehaviourPun
{
    private PhotonView _View;
    
    private void Start()
    {
        _View = gameObject.AddComponent<PhotonView>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendPackage()
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.ToObject(FindObjectOfType<ButtonsScript>().localPlayer);
        
        Debug.Log("Vamos a mandar: " + objToSend);       
        
        _View.RPC("RPCUpdateTurn", RpcTarget.All, (object)objToSend);
    }
    
    #region RPCMethods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    [PunRPC]
    public void RPCUpdateTurn(object[] obj)
    {
        Debug.Log("RPC recibido");
        
        FindObjectOfType<GameManager>().MatchId = obj[0] as string;
        FindObjectOfType<GameManager>().OwnerId = obj[1] as string;
        if (FindObjectOfType<PlayerInfo>().Name == FindObjectOfType<GameManager>().PlayerInfoO.Name)
        {
            FindObjectOfType<GameManager>().PlayerInfoO.Name = obj[2] as string;
        }
        else
        {
            FindObjectOfType<GameManager>().PlayerInfoX.Name = obj[2] as string;
        }
        FindObjectOfType<GameManager>().WhosTurn.Name = obj[3] as string;
        FindObjectOfType<GameManager>().NumFilled = (int)obj[4];
        FindObjectOfType<GameManager>().FilledPositions = (int[,])obj[5];
        FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[7];

        FindObjectOfType<GameManager>().turnMoment = 0;

        FindObjectOfType<ButtonsScript>().UpdateTurn();
    }

    #endregion
}
