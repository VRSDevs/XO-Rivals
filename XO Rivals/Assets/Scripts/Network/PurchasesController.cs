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
    #region Vars

    /// <summary>
    /// ¿Se realizó la compra?
    /// </summary>
    private bool _purchaseCompleted = false;

    #endregion

    #region Getters

    /// <summary>
    /// Método para devolver el valor de _purchaseCompleted
    /// </summary>
    /// <returns>Valor de _purchaseCompleted</returns>
    public bool HasPurchased()
    {
        return _purchaseCompleted;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Método para actualizar el valor
    /// </summary>
    public void UpdatePurchaseStatus()
    {
        _purchaseCompleted = !_purchaseCompleted;
    }

    #endregion

    #region UnityCB

    // Start is called before the first frame update
    void Start()
    {
        InitObject();
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
            
            ProcessPurchase(result.OrderId, item);
        }, error => {
            Debug.Log("No se pudo obtener el item correspondiente");
        });
    }

    /// <summary>
    /// Método para procesar la compra inicializada
    /// </summary>
    /// <param name="order">Código del pedido</param>
    private void ProcessPurchase(string order, ShopItem item)
    {
        Debug.Log("Pedido: " + order);
        PlayFabClientAPI.PayForPurchase(new PayForPurchaseRequest() {
            OrderId = order,
            ProviderName = "PayPal",
            Currency = "RM"
        }, result => {
            Debug.Log("Compra procesada");
            FilterCheckout(item);
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
            
            UpdatePurchaseStatus();
        });
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para llamar a un checkout en función del item a comprar
    /// </summary>
    /// <param name="item">Item a comprar</param>
    private void FilterCheckout(ShopItem item)
    {
        switch (item)
        {
            case ShopItem.Life1:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AXD63Q84BGUVQ");
                break;
            case ShopItem.Lives3:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TD8L8CUL7EUMS");
                break;
            case ShopItem.Lives5:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5224MUUBASLDS");
                break;
            case ShopItem.Lives10:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=BNQVGGLCAALU4");
                break;
            case ShopItem.Lives30:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9VBQRUY2YAYKU");
                break;
            case ShopItem.LivesInf:
                Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HCB7WWEJ87FCW");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item), item, null);
        }
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para inicializar las variables del objeto
    /// </summary>
    private void InitObject()
    {
        _purchaseCompleted = false;
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
