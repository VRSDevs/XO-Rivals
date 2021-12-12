using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;

public class BetaShop : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private TextMeshProUGUI shopText;

    [SerializeField] private MainMenuController _mainController;
    private PlayerInfo _localPlayer;

    #endregion

    #region UnityCB

    private void Start(){
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
    }

    #endregion

    #region PurchaseMethods

    public void OfferBought(int offer)
    {
        if(_localPlayer.Lives < 999){
            shopText.text = "You bought " + offer + " lives";
            
            switch (offer)
            {
                case 1:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Life1);
                    
                    break;
                case 3:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives3);
                    
                    break;
                case 5:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives5);
                    
                    break;
                case 10:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives10);
                    
                    break;
                case 30:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives30);
                    
                    break;
                case 999:
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.LivesInf);

                    break;
            }
            
            StartCoroutine(UpdateLives(offer));
        }else{
            shopText.text = "You can´t buy more lives!";
        }
    }

    public void MinigamesBought(int offer){
        switch(offer){
            
            case 0:
                shopText.text = "You bought 3 minigames";
                break;

            case 1:
                break;
        }
    }

    #endregion

    /// <summary>
    /// Método para actualizar las vidas del jugador
    /// </summary>
    /// <param name="amount">Cantidad de vidas compradas</param>
    private IEnumerator UpdateLives(int amount)
    {
        yield return new WaitUntil(FindObjectOfType<GameManager>().IsPurchaseCompleted);
        Debug.Log("Actualizando vidas");
        
        _localPlayer.Lives = (_localPlayer.Lives + amount) > 999 ? 999 : _localPlayer.Lives + amount;
        _mainController.LivesTxt.text = "Lives: " + _localPlayer.Lives;
        _mainController.LivesTxtShop.text = "Lives: " + _localPlayer.Lives;
        
        FindObjectOfType<GameManager>().UpdateCloudData(new Dictionary<string, string>()
        {
            {DataType.Lives.GetString(), _localPlayer.Lives.ToString()}
        },
            DataType.Lives);
        
        FindObjectOfType<GameManager>().UpdatePurchaseStatus();
    }
}
