using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PlayFab;
using UnityEngine;
using GetTitleDataRequest = PlayFab.ClientModels.GetTitleDataRequest;
using GetTitleDataResult = PlayFab.ClientModels.GetTitleDataResult;
using GetUserDataRequest = PlayFab.ClientModels.GetUserDataRequest;
using GetUserDataResult = PlayFab.ClientModels.GetUserDataResult;
using UpdateUserDataRequest = PlayFab.ClientModels.UpdateUserDataRequest;

#region Enums&Classes

/// <summary>
/// Tipos de datos que se pueden solicitar
/// </summary>
[Serializable]
public enum DataType
{
    /// <summary>
    /// Datos necesarios para el inicio de sesión
    /// </summary>
    Login,
    /// <summary>
    /// Datos necesarios para el cierre de sesión
    /// </summary>
    Logout,
    /// <summary>
    /// Datos de conexión del usuario
    /// </summary>
    Online,
    /// <summary>
    /// Vidas del usuario
    /// </summary>
    Lives,
    /// <summary>
    /// Nivel del usuario
    /// </summary>
    Level,
    /// <summary>
    /// Tiempo de las vida perdida
    /// </summary>
    LifeLost,
    /// <summary>
    /// Partida del jugador
    /// </summary>
    Match
}

/// <summary>
/// Clase auxiliar para el enumerador DataType
/// </summary>
public static class DataTypeExtension
{
    /// <summary>
    /// Método para transformar un valor del enumerador en una cadena de texto
    /// </summary>
    /// <param name="type">Tipo de dato a transformar</param>
    /// <returns>Cadena de texto equivalente al tipo de dato</returns>
    public static string GetString(this DataType type)
    {
        switch (type)
        {
            case DataType.Online:
                return "Online";
            case DataType.Lives:
                return "Lives";
            case DataType.Level:
                return "Level";
            case DataType.LifeLost:
                return "LifeLost";
            case DataType.Match:
                return "Match";
            default:
                return "";
        }
    }
}

#endregion

#region Interfaces

/// <summary>
/// Interfaz de CloudData
/// </summary>
interface ICloudData
{
    #region Properties

    /// <summary>
    /// ¿Se comprobó si el jugador está conectado?
    /// </summary>
    bool CheckedOnline
    {
        get;
        set;
    }
    
    /// <summary>
    /// Objeto de autentificación de datos
    /// </summary>
    AuthObject AuthObject
    {
        get;
        set;
    }

    /// <summary>
    /// ¿Se recibieron los datos del jugador?
    /// </summary>
    bool GotPlayerData
    {
        get;
        set;
    }

    /// <summary>
    /// ¿Se recibieron los datos del juego?
    /// </summary>
    bool GotTitleData
    {
        get;
        set;
    }

    #endregion
    
    #region CloudMethods

    /// <summary>
    /// Método para obtener los datos especificados del jugador
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    void GetPlayerData(DataType type);
    
    /// <summary>
    /// Método para obtener datos especificados del juego
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    void GetTitleData(DataType type);
    
    /// <summary>
    /// Método para enviar datos del jugador a la nube
    /// </summary>
    /// <param name="data">Diccionario de datos a enviar</param>
    /// <param name="type">Tipo de datos enviados</param>
    void SendPlayerData(Dictionary<string, string> data, DataType type);
    
    /*
    /// <summary>
    /// Método para enviar datos del juego a la nube
    /// </summary>
    /// <param name="key">Clave del dato a enviar</param>
    /// <param name="value">Nuevo valor del dato</param>
    /// <param name="type">Tipo de dato a enviar</param>
    void SendTitleData(string key, string value, DataType type);
    */

    #endregion
}

#endregion

public class CloudDataController : MonoBehaviour, ICloudData
{
    #region Vars
    
    ////////////////// VARIABLES DE CONTROL //////////////////
    private bool _checkedOnline;
    private AuthObject _authObject;
    private bool _gotPlayerData;
    private bool _gotTitleData;
    private List<string> _codeMatches;

    ////////////////// DICCIONARIOS DE DATOS //////////////////
    /// <summary>
    /// Diccionario de datos de la nube
    /// </summary>
    private Dictionary<string, string> _cloudData;
    /// <summary>
    /// Diccionario con el estado de envío de datos
    /// </summary>
    private Dictionary<string, string> _sendDataStatus;

    #endregion

    #region Properties

    public bool CheckedOnline
    {
        get => _checkedOnline;
        set => _checkedOnline = value;
    }

    public AuthObject AuthObject
    {
        get => _authObject;
        set => _authObject = value;
    }

    public bool GotPlayerData
    {
        get => _gotPlayerData;
        set => _gotPlayerData = value;
    }

    public bool GotTitleData
    {
        get => _gotTitleData;
        set => _gotTitleData = value;
    }

    #endregion

    #region Getters

    /// <summary>
    /// Método para obtener el diccionario de datos de la nube
    /// </summary>
    /// <returns>Diccionario de datos</returns>
    public Dictionary<string, string> GetDataDictionary()
    {
        return _cloudData;
    }

    /// <summary>
    /// Método para obtener el estado de la operación de envío
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetSendStatus()
    {
        return _sendDataStatus;
    }

    #endregion

    #region UnityCB

    private void Start()
    {
        InitObject();
    }

    #endregion

    #region ClouldMethods

    public void GetPlayerData(DataType type)
    {
        _cloudData = new Dictionary<string, string>();
        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = FindObjectOfType<PlayerInfo>().UserID,
            Keys = null
        }, (result) =>
        {
            OnDataRecieved(result, type);
        }, OnRecieveError);
    }
    
    public void GetTitleData(DataType type)
    {
        _cloudData = new Dictionary<string, string>();
        
        PlayFabClientAPI.GetTitleData(
            new GetTitleDataRequest(), 
            result => OnTitleDataRecieved(result, type),
            error => OnRecieveTitleError(error));
    }
    
    public void SendPlayerData(Dictionary<string, string> data, DataType type)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = data,
        }, (result) =>
        {
            OnDataSend(type);
        }, OnSendError);
    }
    
    /*
    public void SendTitleData(string key, string value, DataType type)
    {
        PlayFabServerAPI.SetTitleData(new SetTitleDataRequest()
        {
            Key = key,
            Value = value
        }, (result) =>
        {
            OnTitleDataSend(type);
        }, OnSendTitleError);
    }
    */

    #endregion

    #region SuccessCB

    /// <summary>
    /// Método ejecutado tras recibir los datos del servidor
    /// </summary>
    /// <param name="result">Resultado de la consulta</param>
    /// <param name="type">Tipo de dato buscado</param>
    /// <returns>Diccionario con los datos solicitados</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void OnDataRecieved(GetUserDataResult result, DataType type)
    {
        if (result == null)
        {
            _cloudData = new Dictionary<string, string>()
            {
                {"ResultCode", "2"}
            };

            _gotPlayerData = true;

            return;
        }

        Debug.Log("Se han recibido datos");
        
        _cloudData = new Dictionary<string, string>()
        {
            {"ResultCode", "1"}
        };
        
        switch (type)
        {
            case DataType.Login:

                if (result.Data.ContainsKey(DataType.Online.GetString()) && bool.Parse(result.Data[DataType.Online.GetString()].Value))
                {
                    _authObject.Failed = true;
                    _authObject.ErrorCode = PlayFabErrorCode.ConnectionError;
                    _authObject.Message = "Account already online";

                    _checkedOnline = true;

                    return;
                }

                _checkedOnline = true;

                // Inserción en diccionario la información básica del jugador
                _cloudData.Add(DataType.Online.GetString(), !result.Data.ContainsKey(DataType.Online.GetString()) ? "false" : result.Data[DataType.Online.GetString()].Value);
                _cloudData.Add(DataType.Lives.GetString(), !result.Data.ContainsKey(DataType.Lives.GetString()) ? "3" : result.Data[DataType.Lives.GetString()].Value);
                _cloudData.Add(DataType.Level.GetString(), !result.Data.ContainsKey(DataType.Level.GetString()) ? "0.0" : result.Data[DataType.Level.GetString()].Value);
                _cloudData.Add(DataType.LifeLost.GetString(), !result.Data.ContainsKey(DataType.LifeLost.GetString()) || string.IsNullOrEmpty(result.Data[DataType.LifeLost.GetString()].Value)
                    ? System.DateTime.Now.ToString(CultureInfo.InvariantCulture) : result.Data[DataType.LifeLost.GetString()].Value);
                
                // Inserción de información sobre partidas
                int totalMatches = !result.Data.ContainsKey(DataType.Match.GetString())
                    ? 0
                    : int.Parse(result.Data[DataType.Match.GetString()].Value);
                _cloudData.Add(DataType.Match.GetString(), totalMatches.ToString());

                for (int i = 0; i < totalMatches; i++)
                {
                    string key = DataType.Match.GetString() + i;
                    string value = result.Data[key].Value;
                    
                    _cloudData.Add(key, value);
                    _codeMatches.Add(value);
                }

                break;
            case DataType.Lives:
                break;
            case DataType.Level:
                break;
            case DataType.LifeLost:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        _gotPlayerData = true;
    }

    /// <summary>
    /// CB ejecutado tras recibir los datos del título
    /// </summary>
    /// <param name="result">Resultado obtenido</param>
    /// <param name="type">Tipo de dato del resultado</param>
    private void OnTitleDataRecieved(GetTitleDataResult result, DataType type)
    {
        // Si no ha devuelto ningún dato
        if (result.Data == null)
        {
            _cloudData = new Dictionary<string, string>()
            {
                {"ResultCode", "2"}
            };
            
            GotTitleData = true;

            return;
        }
        
        _cloudData = new Dictionary<string, string>()
        {
            {"ResultCode", "1"}
        };

        switch (type)
        {
            case DataType.Match:

                foreach (var key in _codeMatches.Select(code => type.GetString() + code).Where(key => result.Data.ContainsKey(key)))
                {
                    _cloudData.Add(key, result.Data[key]);
                }

                break;
        }
        
        GotTitleData = true;
    }
    
    /// <summary>
    /// Método ejecutado cuando se mandan los datos de manera correcta
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    /// <returns>Diccionario con el resultado del envío</returns>
    private void OnDataSend(DataType type)
    {
        _sendDataStatus = new Dictionary<string, string>()
        {
            {"ResultCode", "1"}
        };

        switch (type)
        {
            // Caso Logout -> Procedimiento tras enviar datos de cierre de sesión
            case DataType.Logout:
                PlayFabClientAPI.ForgetAllCredentials();
                
                break;
        }
    }
    
    /// <summary>
    /// CB ejecutado tras enviar los datos del título
    /// </summary>
    /// <param name="type">Tipo de dato enviado</param>
    private void OnTitleDataSend(DataType type)
    {
        _sendDataStatus = new Dictionary<string, string>()
        {
            {"ResultCode", "1"}
        };

        switch (type)
        {
            // Caso Logout -> Procedimiento tras enviar datos de cierre de sesión
            case DataType.Logout:
                //PlayFabClientAPI.ForgetAllCredentials();
                
                break;
        }
    }
    
    #endregion

    #region FailureCB

    /// <summary>
    /// Método CB llamado cuando falla la petición de datos
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    /// <returns>Diccionario con un código de error</returns>
    private void OnRecieveError(PlayFabError error)
    {
        _cloudData = new Dictionary<string, string>()
        {
            {"ResultCode", "2"}
        };

        CheckedOnline = true;
        GotPlayerData = true;
    }
    
    /// <summary>
    /// CB llamado cuando falla la petición de datos del título
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    /// <returns>Diccionario con un código de error</returns>
    private void OnRecieveTitleError(PlayFabError error)
    {
        _cloudData = new Dictionary<string, string>()
        {
            {"ResultCode", "2"}
        };
        
        GotTitleData = true;
    }
    
    /// <summary>
    /// Método CB llamado cuando falla la petición de datos
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    private void OnSendError(PlayFabError error)
    {
        _sendDataStatus = new Dictionary<string, string>()
        {
            {"ResultCode", "3"}
        };
    }
    
    /// <summary>
    /// CB llamado tras fallar el envío de datos del título
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    private void OnSendTitleError(PlayFabError error)
    {
        _sendDataStatus = new Dictionary<string, string>()
        {
            {"ResultCode", "3"}
        };
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para inicializar el objeto
    /// </summary>
    private void InitObject()
    {
        _checkedOnline = false;
        _gotPlayerData = false;
        _gotTitleData = false;
        _codeMatches = new List<string>();

        _cloudData = new Dictionary<string, string>();
        _sendDataStatus = new Dictionary<string, string>();
    }

    /// <summary>
    /// Método para resetear el  objeto
    /// </summary>
    public void ResetObject()
    {
        InitObject();
    }

    #endregion
}
