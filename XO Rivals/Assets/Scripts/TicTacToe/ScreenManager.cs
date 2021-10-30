using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    //Turn Text
    [SerializeField] private Text screenTxt;

    //Timer variable
    private float totalTime = 2f;

    //Control of miniGameChoosing
    [SerializeField] private GameObject miniGameChoosing;
    [SerializeField] private GameObject ticTacScreen;
    [SerializeField] private ButtonsScript buttonsScript;
    public static int minigame = -1;

    //Reference to all butons
    [SerializeField] private Button[] buttonsReference;

    void Start(){

        if(buttonsScript.gameState.WhosTurn == buttonsScript.gameState.PlayerInfoO){
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }        
    }

    public void UpdateTurn(PlayerInfo playerTurn){

        if(buttonsScript.gameState.PlayerInfoO == playerTurn)
            buttonsScript.gameState.WhosTurn = buttonsScript.gameState.PlayerInfoX;
        else
            buttonsScript.gameState.WhosTurn = buttonsScript.gameState.PlayerInfoO;
        
        buttonsScript.gameState.turnMoment = 0;
        //Display turn in screen
        if(buttonsScript.gameState.WhosTurn == buttonsScript.gameState.PlayerInfoO){            
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        } 
    }
    
    public IEnumerator txtTimer(string txt){

        screenTxt.enabled = true;
        screenTxt.text = txt;
        yield return new WaitForSeconds(totalTime);
        screenTxt.enabled = false;

    }

    public void MinigameSelectionActivation(){
        ticTacScreen.SetActive(false);
        miniGameChoosing.SetActive(true);
    }    

    public void MinigameSelection(int n){
        buttonsScript.gameState.miniGameChosen = n;
        miniGameChoosing.SetActive(false);
        ticTacScreen.SetActive(true);

        //Unihide chips
        for(int i = 0; i < buttonsScript.gameState.chips.Count; i++)
            buttonsScript.gameState.chips[i].SetActive(true);

        UpdateTurn(buttonsScript.gameState.WhosTurn);

        //Disable interaction with tictac
        DisableButtons();
    }

    public void DisableButtons(){
        for(int i = 0; i < buttonsReference.Length; i++){
            buttonsReference[i].interactable = false;
        }
    }
}
