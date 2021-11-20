using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BetaShop : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI shopText;


    public void OfferBought(int offer)
    {
        switch (offer)
        {
            case 0:
                shopText.text = "You bought 3 minigames";
                break;
            case 1:
                shopText.text = "You bought " + offer + " lives";
                break;
            case 3:
                shopText.text = "You bought " + offer + " lives";
                break;
            case 10:
                shopText.text = "You bought " + offer + " lives";
                break;
            case 20:
                shopText.text = "You bought " + offer + " lives";
                break;
            case 25:
                shopText.text = "You bought " + offer + " lives";
                break;
            case 999:
                shopText.text = "You bought infinite lives";
                break;
        }



    }

}
