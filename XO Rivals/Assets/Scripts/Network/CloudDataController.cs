using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tipos de datos que se pueden solicitar
///     Login - Datos necesarios para el inicio de sesión
///     Online - ¿Está conectado el usuario?
///     Lives - Vidas del usuario
///     Level - Nivel del usuario
///     LifeLost - Tiempo de las vidas perdidas
/// </summary>
[Serializable]
public enum DataType
{
    Login,
    Logout,
    Online,
    Lives,
    Level,
    LifeLost
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
            default:
                return "";
        }
    }
}

public class CloudDataController : MonoBehaviour
{
    #region Vars

    ////////////////// VARIABLES DE CONTROL //////////////////
    /// <summary>
    /// ¿Se comprobó si está en línea?
    /// </summary>
    private bool _checkedOnline;
    /// <summary>
    /// ¿Se sincronizaron los datos?
    /// </summary>
    private bool _synchronized;
    /// <summary>
    /// Objeto de autentificación
    /// </summary>
    public AuthObject Obj;

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

    #region Getters

    /// <summary>
    /// Método para devolver el valor de la comprobación de estado en línea
    /// </summary>
    /// <returns>Valor de la comrpobación</returns>
    public bool IsOnlineChecked()
    {
        return _checkedOnline;
    }

    /// <summary>
    /// Método para devolver el valor de control de sincronización
    /// </summary>
    /// <returns>Valor de la sincronización</returns>
    public bool IsSynchronized()
    {
        return _synchronized;
    }

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

    #region UpdateMethods

    /// <summary>
    /// Método actualización del estado de comprobación de si el jugador está en línea
    /// </summary>
    private void UpdateOnlineChecked()
    {
        _checkedOnline = !_checkedOnline;
    }

    /// <summary>
    /// Método actualización del estado de sincronización
    /// </summary>
    private void UpdateSynchronizedStatus()
    {
        _synchronized = !_synchronized;
    }

    #endregion

    #region UnityCB

    private void Start()
    {
        InitObject();
    }

    #endregion

    #region ClouldMethods

    /// <summary>
    /// Método para la obtención de datos de la nube
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    /// <returns>Diccionario con los datos solicitados</returns>
    public void GetData(DataType type)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = FindObjectOfType<PlayerInfo>().UserID,
            Keys = null
        }, (result) =>
        {
            OnDataRecieved(result, type);
        }, OnRecieveError);
    }

    /// <summary>
    /// Método para enviar datos
    /// </summary>
    /// <param name="data">Diccionario de datos a enviar</param>
    /// /// <param name="type">Tipo de dato a enviar</param>
    /// <returns>Estado de la operación</returns>
    public void SendData(Dictionary<string, string> data, DataType type)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = data
        }, (result) =>
        {
            OnDataSend(type);
        }, OnSendError);
    }

    #endregion

    #region CBMethods

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
            Debug.Log("Era nulo");
            
            UpdateSynchronizedStatus();
            
            _cloudData = new Dictionary<string, string>()
            {
                {"ResultCode", "2"}
            };

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
                    Obj.Failed = true;
                    Obj.ErrorCode = PlayFabErrorCode.ConnectionError;
                    Obj.Message = "Account already online";
                    
                    UpdateOnlineChecked();

                    return;
                }
                
                UpdateOnlineChecked();

                _cloudData.Add(DataType.Online.GetString(), !result.Data.ContainsKey(DataType.Online.GetString()) ? "false" : result.Data[DataType.Online.GetString()].Value);
                _cloudData.Add(DataType.Lives.GetString(), !result.Data.ContainsKey(DataType.Lives.GetString()) ? "3" : result.Data[DataType.Lives.GetString()].Value);
                _cloudData.Add(DataType.Level.GetString(), !result.Data.ContainsKey(DataType.Level.GetString()) ? "0.0" : result.Data[DataType.Level.GetString()].Value);
                _cloudData.Add(DataType.LifeLost.GetString(), !result.Data.ContainsKey(DataType.LifeLost.GetString()) || string.IsNullOrEmpty(result.Data[DataType.LifeLost.GetString()].Value)
                    ? System.DateTime.Now.ToString(CultureInfo.InvariantCulture) : result.Data[DataType.LifeLost.GetString()].Value);

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
        
        UpdateSynchronizedStatus();
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
    /// Método CB llamado cuando falla la petición de datos
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    /// <returns>Diccionario con un código de error</returns>
    private void OnRecieveError(PlayFabError error)
    {
        Debug.Log("a");
        
        _cloudData = new Dictionary<string, string>()
        {
            {"ResultCode", "2"}
        };
        
        UpdateOnlineChecked();
        UpdateSynchronizedStatus();
    }
    
    /// <summary>
    /// Método CB llamado cuando falla la petición de datos
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    /// <returns>Diccionario con un código de error</returns>
    private void OnSendError(PlayFabError error)
    {
        _sendDataStatus = new Dictionary<string, string>()
        {
            {"ResultCode", "3"}
        };
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para inicializar las variables del objeto
    /// </summary>
    private void InitObject()
    {
        _checkedOnline = false;
        _synchronized = false;
    }

    /// <summary>
    /// Método para resetear las variables del objeto
    /// </summary>
    public void ResetObject()
    {
        InitObject();
    }

    #endregion
}
