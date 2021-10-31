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
        _View.ViewID = 1;
    }

    public void SendPlayerInfoPackage(string type)
    {
        object[] objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(type);
        Debug.Log("Vamos a mandar: " + objToSend);  
        _View.RPC("RPCGetPlayerInfo", RpcTarget.OthersBuffered, (object)objToSend);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendPackage()
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.ToObject(FindObjectOfType<ButtonsScript>().localPlayer);
        Debug.Log("Vamos a mandar: " + objToSend);
        _View.RPC("RPCUpdateTurn", RpcTarget.OthersBuffered, (object)objToSend);
    }
    
    #region RPCMethods
    
    [PunRPC]
    public void RPCGetPlayerInfo(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "host":
                Debug.Log("RPC recibido del host");
                
                FindObjectOfType<GameManager>().MatchId = obj[0] as string;
                FindObjectOfType<GameManager>().OwnerId = obj[1] as string;

                if (FindObjectOfType<GameManager>().PlayerInfoO == null)
                {
                    Debug.Log("No hay información del PlayerO");
                    FindObjectOfType<GameManager>().PlayerInfoO = gameObject.AddComponent<PlayerInfo>();
                    FindObjectOfType<GameManager>().PlayerInfoO.Name = obj[2] as string;
                }

                if (FindObjectOfType<GameManager>().WhosTurn == null)
                {
                    Debug.Log("No hay información del jugador del turno actual");
                    FindObjectOfType<GameManager>().WhosTurn = gameObject.AddComponent<PlayerInfo>();
                    FindObjectOfType<GameManager>().WhosTurn.Name = obj[3] as string;
                }
                
                break;
            case "user":
                Debug.Log("RPC recibido del usuario");
                
                if (FindObjectOfType<GameManager>().PlayerInfoX == null)
                {
                    Debug.Log("No hay información del PlayerO");
                    FindObjectOfType<GameManager>().PlayerInfoX = gameObject.AddComponent<PlayerInfo>();
                    FindObjectOfType<GameManager>().PlayerInfoX.Name = obj[2] as string;
                }
                
                break;
        }
    }

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

        if (FindObjectOfType<GameManager>().PlayerInfoO == null)
        {
            FindObjectOfType<GameManager>().PlayerInfoO = gameObject.AddComponent<PlayerInfo>();
            FindObjectOfType<GameManager>().PlayerInfoO.Name = obj[2] as string;
        }
        else
        {
            FindObjectOfType<GameManager>().PlayerInfoX = gameObject.AddComponent<PlayerInfo>();
            FindObjectOfType<GameManager>().PlayerInfoX.Name = obj[2] as string;
        }

        if (FindObjectOfType<GameManager>().WhosTurn == null)
        {
            FindObjectOfType<GameManager>().WhosTurn = gameObject.AddComponent<PlayerInfo>();
            FindObjectOfType<GameManager>().WhosTurn.Name = obj[3] as string;
        }
        else
        {
            FindObjectOfType<GameManager>().WhosTurn.Name = obj[3] as string;
        }
        
        FindObjectOfType<GameManager>().NumFilled = (int)obj[4];
        FindObjectOfType<GameManager>().FilledPositions = (int[,])obj[5];
        FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[7];

        FindObjectOfType<GameManager>().turnMoment = 0;

        FindObjectOfType<ButtonsScript>().UpdateTurn();
    }

    #endregion
}
