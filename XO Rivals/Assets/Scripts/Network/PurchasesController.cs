using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItem
{
    Life1,
    Lives3,
    Lives5,
    Lives10,
    Lives30,
    LivesInf
}

/// <summary>
/// Clase auxiliar para el enumerador ShopItem
/// </summary>
public static class ShopItemExtension
{
    /// <summary>
    /// Método para transformar un valor del enumerador en una cadena de texto
    /// </summary>
    /// <param name="item">Item del cual obtener el ID</param>
    /// <returns>ID del item solicitado</returns>
    public static string GetString(this ShopItem item)
    {
        switch (item)
        {
            case ShopItem.Life1:
                return "onelfpck";
            case ShopItem.Lives3:
                return "threelvspck";
            case ShopItem.Lives5:
                return "fivelvspck";
            case ShopItem.Lives10:
                return "tenlvspck";
            case ShopItem.Lives30:
                return "thirtylvspck";
            case ShopItem.LivesInf:
                return "inflvspck";
            default:
                return "";
        }
    }

    public static string GetCatalog(this ShopItem item)
    {
        
    }
}

public class PurchasesController : MonoBehaviour
{

    #region UnityCB

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region PurchaseMethods
    
    

    #endregion
    
}
