using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;

public class BetaShop : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI shopText;

    [SerializeField] private MainMenuController _mainController;
    private PlayerInfo _localPlayer;

    private void Start(){
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
    }

    public void OfferBought(int offer)
    {
        if(_localPlayer.Lives != 999){
            switch (offer)
            {
                case 1:
                    shopText.text = "You bought " + offer + " lives";
                    _localPlayer.Lives++;
                    UpdateLives();
                    break;
                case 3:
                    shopText.text = "You bought " + offer + " lives";
                    _localPlayer.Lives += 3;
                    UpdateLives();
                    break;
                case 10:
                    shopText.text = "You bought " + offer + " lives";
                    _localPlayer.Lives += 10;
                    UpdateLives();
                    break;
                case 20:
                    shopText.text = "You bought " + offer + " lives";
                    _localPlayer.Lives += 20;
                    UpdateLives();
                    break;
                case 25:
                    shopText.text = "You bought " + offer + " lives";
                    _localPlayer.Lives += 25;
                    UpdateLives();
                    break;
                case 999:
                    shopText.text = "You bought infinite lives";
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
    private void UpdateLives(){

        _mainController.lifesTxt.text = "Lives: " + _localPlayer.Lives;
        _mainController.lifesTxtShop.text = "Lives: " + _localPlayer.Lives;

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
            _mainController.lifesTime.text = "MAX";
    }
}
