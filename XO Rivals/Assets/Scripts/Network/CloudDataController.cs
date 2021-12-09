using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

/// <summary>
/// Tipos de datos que se pueden solicitar
///     Login - Datos necesarios para el inicio de sesión
///     Lives - Vidas del usuario
///     Level - Nivel del usuario
///     LifeLost - Tiempo de las vidas perdidas
/// </summary>
[Serializable]
public enum DataType
{
    Login,
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
            case DataType.Login:
                return "";
            case DataType.Lives:
                return "Lives";
            case DataType.Level:
                return "Level";
            default:
                return "";
        }
    }
}

public class CloudDataController : MonoBehaviour
{

    #region ClouldMethods

    /// <summary>
    /// Método para la obtención de datos de la nube
    /// </summary>
    /// <param name="type">Tipo de dato a obtener</param>
    /// <returns>Diccionario con los datos solicitados</returns>
    public Dictionary<string, string> GetData(DataType type)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = FindObjectOfType<PlayerInfo>().ID,
            Keys = null
        }, (result) =>
        {
            data = OnDataRecieved(result, type);
        }, (error) =>
        {
            data = OnError(error);
        });

        return data;
    }

    /// <summary>
    /// Método para enviar datos
    /// </summary>
    /// <param name="data">Diccionario de datos a enviar</param>
    /// <returns>Estado de la operación</returns>
    public Dictionary<string, string> SendData(Dictionary<string, string> data)
    {
        Dictionary<string, string> status = new Dictionary<string, string>();
        data.Remove("ResultCode");
        
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = data
        }, (result) =>
        {
            status = OnDataSend();
        }, (error) =>
        {
            status = OnError(error);
        });

        return status;
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
    private Dictionary<string, string> OnDataRecieved(GetUserDataResult result, DataType type)
    {
        if (result == null)
        {
            return new Dictionary<string, string>()
            {
                {"ResultCode", "2"}
            };
        }

        Debug.Log("Se han recibido datos");
        
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"ResultCode", "1"}
        };
        
        switch (type)
        {
            case DataType.Login:

                data.Add(DataType.Lives.GetString(), !result.Data.ContainsKey(DataType.Lives.GetString()) ? "3" : result.Data[DataType.Lives.GetString()].Value);
                data.Add(DataType.Level.GetString(), !result.Data.ContainsKey(DataType.Level.GetString()) ? "0.0" : result.Data[DataType.Level.GetString()].Value);
                data.Add(DataType.LifeLost.GetString(), !result.Data.ContainsKey(DataType.LifeLost.GetString()) && !string.IsNullOrEmpty(result.Data[DataType.LifeLost.GetString()].Value)
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

        return data;
    }

    /// <summary>
    /// Método ejecutado cuando se mandan los datos de manera correcta
    /// </summary>
    /// <returns>Diccionario con el resultado del envío</returns>
    private Dictionary<string, string> OnDataSend()
    {
        return new Dictionary<string, string>()
        {
            { "ResultCode", "1" }
        };
    }

    /// <summary>
    /// Método CB llamado cuando falla la petición de datos
    /// </summary>
    /// <param name="error">Error devuelto por el servidor</param>
    /// <returns>Diccionario con un código de error</returns>
    private Dictionary<string, string> OnError(PlayFabError error)
    {
        return new Dictionary<string, string>()
        {
            { "ResultCode", "3" }
        };
    }

    #endregion
}
