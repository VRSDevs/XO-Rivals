using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

#region Enums

/// <summary>
/// Tipos de datos que se pueden solicitar
/// </summary>
[Serializable]
public enum SendingState
{
    /// <summary>
    /// Datos del jugador de la partida
    /// </summary>
    PlayerInfo,
    /// <summary>
    /// Información de la partida
    /// </summary>
    MatchInfo,
    /// <summary>
    /// Información de final de partida
    /// </summary>
    EndMatchInfo,
}

#endregion

public class NetworkCommunications : MonoBehaviourPun
{
    #region Vars

    private PhotonView _view;

    #endregion
    
    #region UnityCB

    private void Start()
    {
        _view = PhotonView.Get(this);
    }

    #endregion
    
    #region SendingMethods

    /// <summary>
    /// Método para enviar un objeto de información por RPC
    /// </summary>
    /// <param name="data">Diccionario con datos a enviar</param>
    /// <param name="state">Estado en el cual se enviará el RPC</param>
    public void SendRPC(Dictionary<string, string> data, SendingState state)
    {
        object[] objToSend;
        
        switch (state)
        {
            case SendingState.PlayerInfo:
                objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(data["PlayerType"]);
                _view.RPC("PlayerInfoRPC",
                    RpcTarget.Others,
                    (object)objToSend);
                
                break;
            case SendingState.MatchInfo:
                objToSend = FindObjectOfType<ButtonsScript>().gameState.MatchInfoToObject(data["Event"]);
                _view.RPC("RPCUpdateMatch", 
                    RpcTarget.OthersBuffered, 
                    (object)objToSend);

                break;
            case SendingState.EndMatchInfo:
                objToSend = FindObjectOfType<ButtonsScript>().gameState.EndMatchInfoToObject(data["Event"], 
                    data["Winner"]);
                _view.RPC("RPCEndMatch", RpcTarget.OthersBuffered, (object)objToSend);
                
                break;
        }
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

                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).OpponentId = obj[1] as string;
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).PlayerOName = obj[2] as string;
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).WhosTurn = obj[3] as string;

                FindObjectOfType<GameManager>().SetReadyStatus();

                break;
            case "X":
                Debug.Log("RPC del jugador X");

                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).OpponentId = obj[1] as string;
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).PlayerXName = obj[2] as string;

                FindObjectOfType<GameManager>().SetReadyStatus();

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
                Debug.Log(obj[0] as string);
                Debug.Log(obj[1] as string);
                Debug.Log((int)obj[2]);
                Debug.Log((int)obj[3]);
                Debug.Log((int)obj[4]);
                Debug.Log((int)obj[5]);
                Debug.Log((string)obj[6]);
                Debug.Log(obj[7] as string);



                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).NumFilled = (int)obj[2];
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).FilledPositions[
                    (int)obj[3], (int)obj[4]
                ] = (int)obj[5];
                

                FindObjectOfType<ScreenManager>().UpdateBoard(
                    (int)obj[3], 
                    (int)obj[4], 
                    (string)obj[6]
                );
                
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).MiniGameChosen = (int)obj[7];

                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().startGame();
                FindObjectOfType<ButtonsScript>().updateIconTurn(true);
                FindObjectOfType<ButtonsScript>().colocarFichas();
                FindObjectOfType<ButtonsScript>().CheckVictory();




                break;
            case "OppLost":
                
                Debug.Log("RPC oponente perdió");
                
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).MiniGameChosen = (int)obj[2];
                
                FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().startGame();
                FindObjectOfType<ButtonsScript>().updateIconTurn(true);

                break;
        }
    }
    
    /// <summary>
    /// RPC recibido en el usuario con la información del final de la partida
    /// </summary>
    /// <param name="obj">Objeto con la información</param>
    [PunRPC]
    public void RPCEndMatch(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "WN":
            case "DF":
                if (obj[1].Equals(GameObject.Find("PlayerObject").GetComponent<PlayerInfo>().Name))//RECIBE QUIEN GANA
                {
                    FindObjectOfType<EndGameScript>().ShowMatchVictory();
                }
                else
                {
                     FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                     FindObjectOfType<GameManager>().GetMatch(PhotonNetwork.CurrentRoom.Name).SetIsEnded();
                }

                break;
            case "DW":
                    FindObjectOfType<EndGameScript>().ShowMatchDraw();

                break;
            case "SR":
                    FindObjectOfType<EndGameScript>().ShowSurrenderVictory();
                break;

        }
    }

    #endregion
}
