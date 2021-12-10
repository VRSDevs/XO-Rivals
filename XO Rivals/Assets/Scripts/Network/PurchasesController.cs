using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
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

    /// <summary>
    /// Método para obtener el catálogo correspondiente al item
    /// </summary>
    /// <param name="item">Item del cual obtener el catálogo</param>
    /// <returns>Catálogo del item</returns>
    public static string GetCatalog(this ShopItem item)
    {
        switch (item)
        {
            case ShopItem.Life1:
            case ShopItem.Lives3:
            case ShopItem.Lives5:
            case ShopItem.Lives10:
            case ShopItem.Lives30:
            case ShopItem.LivesInf:
                return "LivesCatalog";
            default:
                return "";
        }
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

    /// <summary>
    /// Método para iniciar la compra de un item
    /// </summary>
    /// <param name="item">Item a comprar</param>
    public void StartPurchase(ShopItem item)
    {
        PlayFabClientAPI.StartPurchase(new StartPurchaseRequest() {
            CatalogVersion = item.GetCatalog(),
            Items = new List<ItemPurchaseRequest>() {
                new ItemPurchaseRequest() {
                    ItemId = item.GetString(),
                    Quantity = 1,
                    Annotation = "Purchased via in-game store"
                }
            }
        }, result => {
            Debug.Log("Compra iniciada");
            Debug.Log("Item: " + result.Contents[0].DisplayName);
        }, error => {
            // Handle error
        });
    }

    #endregion

}
