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
        if(_localPlayer.Lives != 999){
            switch (offer)
            {
                case 1:
                    shopText.text = "You bought " + offer + " lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Life1);
                    _localPlayer.Lives++;
                    UpdateLives();
                    break;
                case 3:
                    shopText.text = "You bought " + offer + " lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives3);
                    
                    _localPlayer.Lives += 3;
                    UpdateLives();
                    break;
                case 5:
                    shopText.text = "You bought " + offer + " lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives5);
                    
                    _localPlayer.Lives += 5;
                    UpdateLives();
                    break;
                case 10:
                    shopText.text = "You bought " + offer + " lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives10);
                    
                    _localPlayer.Lives += 10;
                    UpdateLives();
                    break;
                case 30:
                    shopText.text = "You bought " + offer + " lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.Lives30);
                    
                    _localPlayer.Lives += 30;
                    UpdateLives();
                    break;
                case 999:
                    shopText.text = "You bought infinite lives";
                    FindObjectOfType<GameManager>().PurchaseItem(ShopItem.LivesInf);
                    
                    _localPlayer.Lives = 999;
                    UpdateLives();
                    break;
            }
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

    
    private void UpdateLives(){
        
        _mainController.LivesTxt.text = "Lives: " + _localPlayer.Lives;
        _mainController.LivesTxtShop.text = "Lives: " + _localPlayer.Lives;
/*
        //Upload lifes to server
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                Data = new Dictionary<string, string>() {
                    {"Lifes", _localPlayer.Lives.ToString()}}
            },
            result => Debug.Log("Successfully bought user lifes"),
            error => {
                Debug.Log("Got error buying user lifes");
            }
        );
        
        if(_localPlayer.Lives >= 3)
            _mainController.LivesTime.text = "MAX";
            */
    }
}
