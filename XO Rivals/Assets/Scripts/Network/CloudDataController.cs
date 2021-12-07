using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[Serializable]
public enum DataType
{
    Lives,
    Level
}

public class CloudDataController
{

    #region ClouldMethods

    public Dictionary<string, string> GetDataFromCloud(DataType type)
    {
        PlayFabClientAPI.GetUserData();

        return null;
    }

    #endregion

    #region CBMethods

    public Dictionary<string, string> OnDataRecieved(GetUserDataResult result, DataType type)
    {
        if (result == null) return null;
        
        Debug.Log("Se han recibido datos");

        try
        {
            switch (type)
            {
                case DataType.Lives:

                    if (!result.Data.ContainsKey("Lives")) throw new Exception();

                    return new Dictionary<string, string>()
                    {
                        {"Lives", result.Data["Lives"].Value}
                    };
                
                case DataType.Level:

                    break;
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    #endregion
}
