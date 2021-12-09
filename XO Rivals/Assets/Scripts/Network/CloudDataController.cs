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
    LIVES,
    LEVEL
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
            
        });

        return data;
    }

    #endregion

    #region CBMethods

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

                data.Add("Lives", !result.Data.ContainsKey("Lives") ? "3" : result.Data["Lives"].Value);
                data.Add("Level", !result.Data.ContainsKey("Level") ? "0.0" : result.Data["Level"].Value);
                data.Add("LifeLost", !result.Data.ContainsKey("LifeLost") ? System.DateTime.Now.ToString(CultureInfo.InvariantCulture) : result.Data["Life Lost"].Value);
                
                break;
            /*
            case DataType.LIVES:

                 throw new Exception();

                return new Dictionary<string, string>()
                {
                    {"Lives", result.Data["Lives"].Value}
                };
            
            case DataType.LEVEL:

                break;
                */
        }

        return data;
    }

    #endregion
}
