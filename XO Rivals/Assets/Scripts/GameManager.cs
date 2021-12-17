using System;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;

#region Interfaces

interface IGameManager
{
    #region Properties

    ////////////////// SERVIDOR //////////////////
    /// <summary>
    /// Referencia al controlador de la conexión con el servidor
    /// </summary>
    NetworkController NetworkController { get; set; }
    
    /// <summary>
    /// Referencia al controlador de comunicaciones
    /// </summary>
    NetworkCommunications NetworkCommunications { get; set; }
    
    /// <summary>
    /// Referencia al controlador de datos de la nube
    /// </summary>
    CloudDataController CloudDataController { get; set; }
    
    /// <summary>
    /// Referencia al controlador de compras
    /// </summary>
    PurchasesController PurchasesController { get; set; }
    
    ////////////////// CLIENTE //////////////////
    /// <summary>
    /// ¿En qué versión de WebGL está?
    /// </summary>
    bool IsWebGLMobile { get; set; }
    
    /// <summary>
    /// ¿Está en matchmaking?
    /// </summary>
    bool InMatchmaking { get; set; }

    #endregion

    #region Getters

    /// <summary>
    /// Método para obtener una partida del diccionario de partidas
    /// </summary>
    /// <param name="key">Clave de la partida a obtener</param>
    /// <returns>Información de la partida</returns>
    Match GetMatch(string key);

    /// <summary>
    /// Método para obtener el diccionario de partidas
    /// </summary>
    /// <returns>Diccionario de partidas</returns>
    Dictionary<string, Match> GetMatches();

    #endregion

    #region Setters

    /// <summary>
    /// Método para agregar una partida al diccionario de partidas
    /// </summary>
    /// <param name="key">Clave de la partida a agregar</param>
    /// <param name="match">Partida a agregar</param>
    void SetMatch(string key, Match match);

    #endregion

    #region ConnectionMethods

    /// <summary>
    /// Método para conectarse al servidor
    /// </summary>
    /// <returns>¿Se conectó al servidor?</returns>
    bool ConnectToServer();

    /// <summary>
    /// Método para desconectarse del servidor
    /// </summary>
    void DisconnectFromServer();

    /// <summary>
    /// Método para conectarse a la lobby del juego
    /// </summary>
    void ConnectToLobby();

    /// <summary>
    /// Método para conectarse a una sala
    /// </summary>
    /// <param name="join">Modo de unión a sala</param>
    /// <param name="code">Código de la sala</param>
    void ConnectToMatch(JoinType join, string code);

    /// <summary>
    /// Método para abandonar una sala
    /// </summary>
    void LeaveMatch();

    #endregion

    #region MatchManagementMethods

    /// <summary>
    /// Método para crear una sala privada
    /// </summary>
    /// <param name="code">Código de la sala</param>
    void CreatePrivateMatch(string code);

    /// <summary>
    /// Método para configurar y añadir una partida
    /// </summary>
    /// <param name="playerType">Tipo de jugador (ficha) que crea la partida</param>
    void SetupMatch(string playerType);

    #endregion
    
    #region CommsMethods
    
    
    
    #endregion

    #region CloudMethods

    /// <summary>
    /// Método para obtener datos de la nube
    /// </summary>
    /// <param name="type">Tipo de datos a obtener</param>
    void GetCloudData(DataType type);

    /// <summary>
    /// Método para enviar datos a la nube
    /// </summary>
    /// <param name="data">Diccionario de datos a enviar</param>
    /// <param name="type">Tipo de datos a enviar</param>
    void UpdateCloudData(Dictionary<string, string> data, DataType type);

    #endregion

    #region PurchaseMethods

    /// <summary>
    /// Método para realizar la compra de un item
    /// </summary>
    /// <param name="item">Item a comprar</param>
    void PurchaseItem(ShopItem item);

    #endregion

    #region CoversionMethods

    /// <summary>
    /// Método para transformar la información del jugador en objeto
    /// </summary>
    /// <param name="playerType">Tipo de jugador (ficha)</param>
    /// <returns>Objeto con la información</returns>
    object[] PlayerInfoToObject(string playerType);

    /// <summary>
    /// Método para transformar la información de la partida en objeto
    /// </summary>
    /// <param name="type">Tipo de información a enviar (minijuego)</param>
    /// <returns>Objeto con la información de la partida</returns>
    object[] MatchInfoToObject(string type);

    /// <summary>
    /// Método para transformar la información del final de la partida en objeto
    /// </summary>
    /// <param name="playerType">Tipo de jugador (ficha)</param>
    /// <returns>Objeto con la información del final de partida</returns>
    object[] EndMatchInfoToObject(string playerType, string winner);

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para inicializar el objeto
    /// </summary>
    void InitObject();

    /// <summary>
    /// Método para resetear el objeto al estado inicial
    /// </summary>
    void ResetObject();

    #endregion
}

#endregion

public class GameManager : MonoBehaviour, IGameManager
{
    #region Vars
    
    ////////////////// REFERENCIAS //////////////////
    [SerializeField] public TMP_Text log;
    
    ////////////////// SERVIDOR //////////////////
    private NetworkController _networkController;
    private NetworkCommunications _networkCommunications;
    private CloudDataController _cloudController;
    private PurchasesController _purchasesController;
    
    ////////////////// CLIENTE //////////////////
    private bool _isWebGLMobile;
    private bool _inMatchmaking;
    
    ////////////////// PARTIDA //////////////////
    /// <summary>
    /// Lista de partidas del jugador
    /// </summary>
    private Dictionary<string, Match> _playerMatches;
    
    ////////////////// USUARIO //////////////////
    /// <summary>
    /// ¿Está jugando?
    /// </summary>
    [NonSerialized] public bool IsPlaying;

    #endregion

    #region Properties

    public NetworkController NetworkController
    {
        get => _networkController; 
        set => _networkController = value;
    }

    public NetworkCommunications NetworkCommunications
    {
        get => _networkCommunications;
        set => _networkCommunications = value;
    }

    public CloudDataController CloudDataController
    {
        get => _cloudController;
        set => _cloudController = value;
    }

    public PurchasesController PurchasesController
    {
        get => _purchasesController;
        set => _purchasesController = value;
    }

    public bool IsWebGLMobile { 
        get => _isWebGLMobile;
        set => _isWebGLMobile = value;
    }

    public bool InMatchmaking
    {
        get => _inMatchmaking;
        set => _inMatchmaking = value;
    }

    #endregion
    
    #region Getters
    
    public Match GetMatch(string key)
    {
        return _playerMatches[key];
    }

    public Dictionary<string, Match> GetMatches()
    {
        return _playerMatches;
    }

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
        return _cloudController.AuthObject;
    }

    /// <summary>
    /// Método para reiniciar el objeto de validación de estado de conexión
    /// </summary>
    public void ResetOnlineAuth()
    {
        _cloudController.AuthObject = new AuthObject();
    }

    /// <summary>
    /// Método para obtener si se está creando una partida
    /// </summary>
    /// <returns>¿Se está creando la partida?</returns>
    public bool GetCheckedOnline()
    {
        return _cloudController.CheckedOnline;
    }

    /// <summary>
    /// Método para obtener el estado de sincronización
    /// </summary>
    /// <returns>Estado de sincronización</returns>
    public bool GetSynchronizeStatus()
    {
        return _cloudController.GotPlayerData;
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
    
    public void SetMatch(string key, Match match)
    {
        _playerMatches.Add(key, match);
    }
    
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

    #region UnityCB
    
    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length < 2)
        {
            InitObject();

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region ConnectionMethods
    
    public bool ConnectToServer()
    {
        return _networkController.ConnectToServer();
    }
    
    public void DisconnectFromServer()
    {
        _networkController.DisconnectFromServer();
    }
    
    public void ConnectToLobby()
    {
        _networkController.ConnectToLobby();
    }
    
    public void ConnectToMatch(JoinType join, string code)
    {
        switch (join)
        {
            case JoinType.RandomRoom:
                _networkController.ConnectToRandomRoom();
                break;
            case JoinType.ActiveRoom:
                _networkController.ConnectToSpecificRoom(code);
                break;
            case JoinType.PrivateRoom:
                _networkController.ConnectToPrivateRoom(code);
                break;
        }
        
    }

    public void LeaveMatch()
    {
        _networkController.DisconnectFromRoom();
    }

    #endregion

    #region MatchMethods
    
    public void CreatePrivateMatch(string code)
    {
        _networkController.CreatePrivateRoom(code);
    }
    
    public void SetupMatch(string playerType)
    {
        _playerMatches.Add(PhotonNetwork.CurrentRoom.Name, new Match());
        _playerMatches[PhotonNetwork.CurrentRoom.Name].MatchId = PhotonNetwork.CurrentRoom.Name;
        
        switch (playerType)
        {
            case "O":
                _playerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName = FindObjectOfType<PlayerInfo>().Name;
                _playerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = FindObjectOfType<PlayerInfo>().Name;

                break;
            case "X":
                _playerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName = FindObjectOfType<PlayerInfo>().Name;

                break;
        }
        
        _networkCommunications.SendPlayerInfoPackage(playerType);
    }

    #endregion
    
    #region CloudMethods
    
    public void GetCloudData(DataType type)
    {
        _cloudController.GetPlayerData(type);
    }
    
    public void UpdateCloudData(Dictionary<string, string> data, DataType type)
    {
        _cloudController.SendPlayerData(data, type);
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
    
    public void PurchaseItem(ShopItem item)
    {
        _purchasesController.StartPurchase(item);
    }

    #endregion

    #region ConversiontMethods
    
    public object[] PlayerInfoToObject(string playerType)
    {
        switch (playerType)
        {
            case "O":
                object[] objO = new object[4];

                objO[0] = playerType;
                objO[1] = FindObjectOfType<PlayerInfo>().UserID;
                objO[2] = _playerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName;
                objO[3] = _playerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;

                return objO;
            case "X":
                object[] objX = new object[3];

                objX[0] = playerType;
                objX[1] = FindObjectOfType<PlayerInfo>().UserID;
                objX[2] = _playerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName;

                return objX;
        }

        return null;
    }
    
    public object[] MatchInfoToObject(string type)
    {
        switch (type)
        {
            case "OppWon":
                object[] objOppWon = new object[8];

                objOppWon[0] = type;
                objOppWon[1] = _playerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;
                objOppWon[2] = _playerMatches[PhotonNetwork.CurrentRoom.Name].NumFilled;
                objOppWon[3] = FindObjectOfType<ButtonsScript>().thisMatch.ActualChip%3;
               
                objOppWon[4] = FindObjectOfType<ButtonsScript>().thisMatch.ActualChip /3;
                
                objOppWon[5] = _playerMatches[PhotonNetwork.CurrentRoom.Name].FilledPositions[
                    FindObjectOfType<ButtonsScript>().thisMatch.ActualChip % 3,
                   FindObjectOfType<ButtonsScript>().thisMatch.ActualChip / 3
                ];
                objOppWon[6] = FindObjectOfType<ButtonsScript>().SelectedTile;
                objOppWon[7] = _playerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen;
                
                return objOppWon;
            case "OppLost":
                object[] objOppLost = new object[3];

                objOppLost[0] = type;
                objOppLost[1] = _playerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;
                objOppLost[2] = _playerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen;
                
                return objOppLost;
        }

        return null;
    }

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

    public void InitObject()
    {
        _networkController = gameObject.AddComponent<NetworkController>();
        _networkCommunications = gameObject.AddComponent<NetworkCommunications>();
        _cloudController = gameObject.AddComponent<CloudDataController>();
        _purchasesController = gameObject.AddComponent<PurchasesController>();
        
        gameObject.AddComponent<PhotonView>();
        GetComponent<PhotonView>().ViewID = 1;
        
        _playerMatches = new Dictionary<string, Match>();
        _inMatchmaking = true;
        IsPlaying = false;
    }

    public void ResetObject()
    {
        _networkController.ResetObject();
        _cloudController.ResetObject();
        _purchasesController.ResetObject();

        _playerMatches.Clear();
        _inMatchmaking = true;
        IsPlaying = false;
    }

    #endregion
}
