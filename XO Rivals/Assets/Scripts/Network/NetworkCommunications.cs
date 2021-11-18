using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCommunications : MonoBehaviourPun
{
    #region Vars

    private PhotonView _View;

    #endregion
    
    #region UnityCB

    private void Start()
    {
        _View = gameObject.AddComponent<PhotonView>();
        _View.ViewID = 1;
    }

    #endregion
    
    #region SendingMethods
    
    /// <summary>
    /// Método para enviar información del jugador al oponente
    /// </summary>
    /// <param name="playerType">Tipo del jugador (en partida)</param>
    public void SendPlayerInfoPackage(string playerType)
    {
        object[] objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(playerType);
        _View.RPC("PlayerInfoRPC", RpcTarget.Others, (object)objToSend);
    }
    
    /// <summary>
    /// Método para enviar información del estado de la partida al oponente
    /// </summary>
    public void SendMatchInfo(string type)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.MatchInfoToObject(type);
        _View.RPC("RPCUpdateMatch", RpcTarget.All, (object)objToSend);
    }
    
    /// <summary>
    /// Método para enviar información de la finalización de la partida
    /// </summary>
    /// <param name="type"></param>
    /// <param name="winner"></param>
    public void SendEndMatchInfo(string type, string winner)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.EndMatchInfoToObject(type, winner);
        _View.RPC("RPCEndMatch", RpcTarget.All, (object)objToSend);
    }
    
    #endregion

    #region RPCMethods
    
    /// <summary>
    /// RPC recibido en el usuario con la información del jugador contrario
    /// </summary>
    /// <param name="obj">Objeto con la información</param>
    [PunRPC]
    public void PlayerInfoRPC(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "O":
                Debug.Log("RPC del jugador O");

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[2] as string;

                FindObjectOfType<GameManager>().UpdateReadyStatus();

                break;
            case "X":
                Debug.Log("RPC del jugador X");

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName = obj[1] as string;

                FindObjectOfType<GameManager>().UpdateReadyStatus();

                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    [PunRPC]
    public void RPCUpdateMatch(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "OppWon":
                
                Debug.Log("RPC oponente ganó");
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].NumFilled = (int)obj[2];
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].FilledPositions[
                    (int)obj[3], (int)obj[4]
                ] = (int)obj[5];
                

                FindObjectOfType<ScreenManager>().UpdateBoard(
                    (int)obj[3], 
                    (int)obj[4], 
                    (string)obj[6]
                );
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen = (int)obj[7];

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
            case "OppLost":
                
                Debug.Log("RPC oponente perdió");
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen = (int)obj[2];
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
        }
    }
    
    public void RPCEndMatch(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "win":
            case "defeat":

                if (obj[1].Equals(GameObject.Find("PlayerObject").GetComponent<PlayerInfo>().Name))
                {
                    FindObjectOfType<EndGameScript>().ShowMatchVictory();
                }
                else
                {
                    FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                }

                break;
            case "draw":
                
                FindObjectOfType<EndGameScript>().ShowMatchDraw();
                
                break;
        }
    }

    #endregion
}
