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
            Debug.Log("Compra iniciada con código: " + result.OrderId);
            Debug.Log("Item: " + result.Contents[0].DisplayName);
            
            ProcessPurchase(result.OrderId);
        }, error => {
            Debug.Log("No se pudo obtener el item correspondiente");
        });
    }

    /// <summary>
    /// Método para procesar la compra inicializada
    /// </summary>
    /// <param name="order">Código del pedido</param>
    private void ProcessPurchase(string order)
    {
        Debug.Log("Pedido: " + order);
        PlayFabClientAPI.PayForPurchase(new PayForPurchaseRequest() {
            OrderId = order,
            ProviderName = "PayPal",
            Currency = "RM"
        }, result => {
            Debug.Log("Compra procesada");
            Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AXD63Q84BGUVQ");
            ConfirmPurchase(order);

        }, error => {
            Debug.Log("Error " + error.Error + ": " + error.ErrorMessage);
        });
    }

    /// <summary>
    /// Método de confirmación de compra
    /// </summary>
    private void ConfirmPurchase(string order)
    {
        PlayFabClientAPI.ConfirmPurchase(new ConfirmPurchaseRequest() {
            OrderId = order
        }, result => {
            Debug.Log("Compra realizada. Gracias!");
            
        }, error => {
            Debug.Log("Error " + error.Error + ": " + error.ErrorMessage);
        });
    }

    #endregion

}
