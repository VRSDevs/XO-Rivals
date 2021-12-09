using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[Serializable]
public enum DataType
{
    Login,
    Lives,
    Level,
    LifeLost
}

public static class DataTypeExtension
{
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

    public Dictionary<string, string> GetLoginData(DataType type)
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
                data.Add(DataType.LifeLost.GetString(), !result.Data.ContainsKey(DataType.LifeLost.GetString()) ? System.DateTime.Now.ToString(CultureInfo.InvariantCulture) : result.Data[DataType.LifeLost.GetString()].Value);
                
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

    private Dictionary<string, string> OnError(PlayFabError error)
    {
        return new Dictionary<string, string>()
        {
            { "ResultCode", "3" }
        };
    }

    #endregion
}
