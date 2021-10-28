using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviourPunCallbacks
{
    #region Variables

    /// <summary>
    /// 
    /// </summary>
    private const int MIN_PLAYERS_IN_ROON = 2;
    /// <summary>
    /// 
    /// </summary>
    private const int MAX_PLAYERS_INROOM = 2;    

    #endregion
    
    #region ConnectMethods

    /// <summary>
    /// 
    /// </summary>
    public bool OnConnectToServer()
    {
        if (PhotonNetwork.ConnectUsingSettings())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnConnectToLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void OnCreateRoom()
    {
        Debug.Log("Creando sala...");

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions()
            {MaxPlayers = MAX_PLAYERS_INROOM}))
        {
            Debug.Log("Sala creada con éxito");
            
        }
        else
        {
            Debug.Log("Fallo al crear la sala");
        }
    }
    
    

    public void OnConnectToRandomRoom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Debug.Log("Fallo al crear la sala");
        }
    }

    #endregion

    #region PUN_CB

    /// <summary>
    /// 
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        Debug.Log("Conectado al servidor. Conectando al lobby...");
        OnConnectToLobby();
        
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        Debug.Log("Conectado al lobby general.");
        Debug.Log("Hola, " + PhotonNetwork.NickName);

        SceneManager.LoadScene("MainMenu");
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        Debug.Log("Unido a la sala.");
    }    

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(returnCode + ": " + message);
        Debug.Log("No existen salas. Creando...");

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions()
            {MaxPlayers = MAX_PLAYERS_INROOM}))
        {
            Debug.Log("Sala creada con éxito");
            
        }
        else
        {
            Debug.Log("Fallo al crear la sala");
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        Debug.Log("Buscando jugadores...");

        if (PhotonNetwork.CurrentRoom.PlayerCount == MAX_PLAYERS_INROOM)
        {
            Debug.Log("Sala llena. Empezando partida...");
            
            SceneManager.LoadScene("TicTacToe_Server");
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    #endregion

    #region UpdateNetworkVars

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nick"></param>
    public void SetNickName(string nick)
    {
        PhotonNetwork.NickName = nick;
    }

    #endregion
    
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
