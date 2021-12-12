using System;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] public TMP_Text log;
    
    ////////////////// SERVIDOR //////////////////
    /// <summary>
    /// Referencia a funciones del servidor
    /// </summary>
    private NetworkController _networkController;
    /// <summary>
    /// 
    /// </summary>
    public NetworkCommunications _networkCommunications;
    /// <summary>
    /// 
    /// </summary>
    private CloudDataController _cloudController;
    /// <summary>
    /// 
    /// </summary>
    private PurchasesController _purchasesController;
    
    ////////////////// PARTIDA //////////////////
    /// <summary>
    /// Lista de partidas del jugador
    /// </summary>
    public Dictionary<string, Match> PlayerMatches;
    
    ////////////////// USUARIO //////////////////
    /// <summary>
    /// ¿En qué versión de WebGL está?
    /// </summary>
    [NonSerialized] public bool IsWebGLMobile;
    /// <summary>
    /// ¿Está jugando?
    /// </summary>
    [NonSerialized] public bool IsPlaying;
    /// <summary>
    /// ¿Está buscando partida?
    /// </summary>
    [NonSerialized] public bool Matchmaking;

    #endregion

    #region UnityCB
    
    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length < 2)
        {
            Debug.Log("No hay GameManager duplicado");
            
            _networkController = gameObject.AddComponent<NetworkController>();
            _networkCommunications = gameObject.AddComponent<NetworkCommunications>();
            gameObject.AddComponent<PhotonView>();
            GetComponent<PhotonView>().ViewID = 1;
            _cloudController = gameObject.AddComponent<CloudDataController>();
            _purchasesController = gameObject.AddComponent<PurchasesController>();
            
            PlayerMatches = new Dictionary<string, Match>();
            Matchmaking = true;
            IsPlaying = false;

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Getters

    /// <summary>
    /// Método para obtener si se está conectado al servidor
    /// </summary>
    /// <returns>¿Está conectado al servidor?</returns>
    public bool GetConnected()
    {
        return _networkController.IsConnected();
    }

    /// <summary>
    /// Método para obtener si se está creando una partida
    /// </summary>
    /// <returns>¿Se está creando la partida?</returns>
    public bool GetIsCreatingMatch()
    {
        return _networkController.GetCreatingRoom();
    }

    /// <summary>
    /// Método para obtener el autentificador de usuario conectado
    /// </summary>
    /// <returns>Objeto de autentificación de usuario conectado</returns>
    public AuthObject GetOnlineAuth()
    {
        return _cloudController.Obj;
    }

    /// <summary>
    /// Método para reiniciar el objeto de validación de estado de conexión
    /// </summary>
    public void ResetOnlineAuth()
    {
        _cloudController.Obj = new AuthObject();
    }

    /// <summary>
    /// Método para obtener si se está creando una partida
    /// </summary>
    /// <returns>¿Se está creando la partida?</returns>
    public bool GetCheckedOnline()
    {
        return _cloudController.IsOnlineChecked();
    }

    /// <summary>
    /// Método para obtener el estado de sincronización
    /// </summary>
    /// <returns>Estado de sincronización</returns>
    public bool GetSynchronizeStatus()
    {
        return _cloudController.IsSynchronized();
    }

    /// <summary>
    /// Método para obtener el código de la sala
    /// </summary>
    /// <returns>Estado de sincronización</returns>
    public string GetRoomCode()
    {
        return _networkController.GenerateRoomCode();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetDataDictionary()
    {
        return _cloudController.GetDataDictionary();
    }

    public Dictionary<string, string> GetSendStatus()
    {
        return _cloudController.GetSendStatus();
    }

    #endregion
    
    #region Setters
    
    /// <summary>
    /// Método para actualizar el nick del cliente en Photon
    /// </summary>
    /// <param name="nick">Nick del cliente</param>
    public void SetPhotonNick(string nick)
    {
        _networkController.SetNickName(nick);
    }
    
    /// <summary>
    /// Método para actualizar el estado del jugador para empezar la partida
    /// </summary>
    public void SetReadyStatus()
    {
        _networkController.UpdateReadyStatus();
    }

    #endregion

    #region ConnectionMethods

    /// <summary>
    /// Método para conectarse al servidor de Photon
    /// </summary>
    /// <returns>Devuelve "true" si el cliente pudo establecer conexión con el servidor</returns>
    public bool OnConnectToServer()
    {
        return _networkController.ConnectToServer();
    }

    /// <summary>
    /// Método para desconectarse del servidor
    /// </summary>
    public void OnDisconnectToServer()
    {
        _networkController.DisconnectFromServer();
    }
    
    /// <summary>
    /// Método para conectarse a la lobby general
    /// </summary>
    public void OnConnectToLobby()
    {
        _networkController.ConnectToLobby();
    }
    
    /// <summary>
    /// Método para conectarse a una sala en Photon
    /// </summary>
    public void OnConnectToRoom()
    {
        _networkController.ConnectToRandomRoom();
    }

    /// <summary>
    /// Método para crear la sala privada
    /// </summary>
    /// <param name="code">Código de la sala</param>
    public void CreatePrivateRoom(string code)
    {
        _networkController.CreatePrivateRoom(code);
    }

    /// <summary>
    /// Método para conectarse a una sala específica en Photon
    /// </summary>
    /// <param name="name"></param>
    public void OnConnectToSpecificRoom(string name)
    {
        _networkController.ConnectToSpecificRoom(name);
    }

    /// <summary>
    /// Método para conectarse a una sala privada
    /// </summary>
    /// <param name="code">Código de la sala</param>
    public void ConnectToPrivateRoom(string code)
    {
        _networkController.ConnectToPrivateRoom(code);
    }

    /// <summary>
    /// Método para abandonar una sala en Photon
    /// </summary>
    public void OnLeaveRoom()
    {
        _networkController.DisconnectFromRoom();
    }

    #endregion

    #region MatchMethods

    /// <summary>
    /// Método para configurar partida y enviar los datos necesarios
    /// </summary>
    /// <param name="playerType">Tipo del jugador (ficha)</param>
    public void SetupMatch(string playerType)
    {
        PlayerMatches.Add(PhotonNetwork.CurrentRoom.Name, new Match());
        
        switch (playerType)
        {
            case "O":
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName = FindObjectOfType<PlayerInfo>().Name;
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = FindObjectOfType<PlayerInfo>().Name;

                break;
            case "X":
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName = FindObjectOfType<PlayerInfo>().Name;

                break;
        }
        
        _networkCommunications.SendPlayerInfoPackage(playerType);
    }

    #endregion
    
    #region CloudMethods

    /// <summary>
    /// Método para obtener datos de la nube
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    /// <returns>Diccionario con los datos solicitados</returns>
    public void GetCloudData(DataType type)
    {
        _cloudController.GetData(type);
    }

    /// <summary>
    /// Método para cargar datos en la nube
    /// </summary>
    /// <param name="data">Datos a cargar</param>
    /// <returns>Estado de la operación</returns>
    public void UpdateCloudData(Dictionary<string, string> data, DataType type)
    {
        _cloudController.SendData(data, type);
    }

    #endregion

    #region PurchaseMethods

    /// <summary>
    /// ¿Completó la compra?
    /// </summary>
    /// <returns>Estado de la compra</returns>
    public bool IsPurchaseCompleted()
    {
        return _purchasesController.HasPurchased();
    }

    /// <summary>
    /// Método para actualizar el estado de compra
    /// </summary>
    public void UpdatePurchaseStatus()
    {
        _purchasesController.UpdatePurchaseStatus();
    }
    
    /// <summary>
    /// Método para realizar la compra de un item de la tienda
    /// </summary>
    /// <param name="item">Item a comprar</param>
    public void PurchaseItem(ShopItem item)
    {
        _purchasesController.StartPurchase(item);
    }

    #endregion

    #region ConversiontMethods
    
    /// <summary>
    /// Método para convertir los datos del jugador en un objeto a enviar
    /// </summary>
    /// <param name="playerType">Tipo del jugador (ficha)</param>
    /// <returns>Objeto de datos</returns>
    public object[] PlayerInfoToObject(string playerType)
    {
        switch (playerType)
        {
            case "O":
                object[] objO = new object[4];

                objO[0] = playerType;
                objO[1] = FindObjectOfType<PlayerInfo>().UserID;
                objO[2] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName;
                objO[3] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;

                return objO;
            case "X":
                object[] objX = new object[3];

                objX[0] = playerType;
                objX[1] = FindObjectOfType<PlayerInfo>().UserID;
                objX[2] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName;

                return objX;
        }

        return null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="localPlayer"></param>
    /// <returns></returns>
    public object[] MatchInfoToObject(string type)
    {
        switch (type)
        {
            case "OppWon":
                object[] objOppWon = new object[8];

                objOppWon[0] = type;
                objOppWon[1] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;
                objOppWon[2] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].NumFilled;
                objOppWon[3] = FindObjectOfType<ButtonsScript>().thisMatch.ActualChip%3;
               
                objOppWon[4] = FindObjectOfType<ButtonsScript>().thisMatch.ActualChip /3;
                
                objOppWon[5] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].FilledPositions[
                    FindObjectOfType<ButtonsScript>().thisMatch.ActualChip % 3,
                   FindObjectOfType<ButtonsScript>().thisMatch.ActualChip / 3
                ];
                objOppWon[6] = FindObjectOfType<ButtonsScript>().SelectedTile;
                objOppWon[7] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen;

                for(int i = 0; i < objOppWon.Length; i++){
                    Debug.Log("Envio turno ganar " + i + ": " + objOppWon[i]);
                }
                return objOppWon;
            case "OppLost":
                object[] objOppLost = new object[3];
                
                Debug.Log(PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn);
                
                objOppLost[0] = type;
                objOppLost[1] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;
                objOppLost[2] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen;
                
                for(int i = 0; i < objOppLost.Length; i++){
                    Debug.Log("Envio turno perder " + i + ": " + objOppLost[i]);
                }
                return objOppLost;
        }

        return null;
    }

    /// <summary>
    /// Método para convertir los datos del final de la partida en un objeto a enviar
    /// </summary>
    /// <param name="type">Tipo de final</param>
    /// <param name="winner">Ganador de la partida</param>
    /// <returns>Objeto a enviar</returns>
    public object[] EndMatchInfoToObject(string type, string winner)
    {
        object[] obj;
        
        switch (type)
        {
            case "WN":
            case "DF":
                obj = new object[2];

                obj[0] = type;
                obj[1] = winner;

                return obj;
            case "DW":
                obj = new object[1];
                
                obj[0] = type;
                
                return obj;
            
            case "SR":
                obj = new object[1];
                
                obj[0] = type;
                
                return obj;
        }

        return null;
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para resetear las variables del objeto
    /// </summary>
    public void ResetObject()
    {
        _networkController.ResetObject();
        _cloudController.ResetObject();
        _purchasesController.ResetObject();

        PlayerMatches.Clear();
        Matchmaking = true;
        IsPlaying = false;
    }

    #endregion
}
