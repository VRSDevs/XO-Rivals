using System.Collections;
using Photon.Pun;
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

        if(buttonsScript.gameState.WhosTurn == buttonsScript.gameState.PlayerInfoO.Name){
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }
    }

    public void UpdateTurn(){

        Debug.Log("X: " + buttonsScript.gameState.PlayerInfoX.Name);
        Debug.Log("O: " + buttonsScript.gameState.PlayerInfoO.Name);
        
        if(buttonsScript.gameState.PlayerInfoO.Name == buttonsScript.gameState.WhosTurn)
            buttonsScript.gameState.WhosTurn = buttonsScript.gameState.PlayerInfoX.Name;
        else
            buttonsScript.gameState.WhosTurn = buttonsScript.gameState.PlayerInfoO.Name;
        
        buttonsScript.gameState.turnMoment = 0;
        //Display turn in screen
        if(buttonsScript.gameState.WhosTurn == buttonsScript.gameState.PlayerInfoO.Name){            
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }

        if (buttonsScript.miniWin)
        {
            buttonsScript.gameState._NetworkCommunications.SendMatchInfo("OppWon");
        }
        else
        {
            buttonsScript.gameState._NetworkCommunications.SendMatchInfo("OppLost");
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
        buttonsScript.gameState.MiniGameChosen = n;
        miniGameChoosing.SetActive(false);
        ticTacScreen.SetActive(true);

        //Unihide chips
        for (int i = 0; i < buttonsScript.gameState.Chips.Count; i++)
        {
            if(buttonsScript.gameState.Chips[i] == null)
                continue;
            
            buttonsScript.gameState.Chips[i].SetActive(true);
        }
        
        UpdateTurn();

        //Disable interaction with tictac
        DisableButtons();
    }

    public void UpdateBoard()
    {
        for(int i = 0; i < buttonsScript.gameState.FilledPositions.GetLength(0); i++){
            for(int j = 0; j < buttonsScript.gameState.FilledPositions.GetLength(1); j++){

                switch (buttonsScript.gameState.FilledPositions[i,j])
                {
                    case 0:
                        buttonsScript.actualChip = Instantiate(buttonsScript.circleGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                        buttonsScript.actualChip.SetActive(true);
                        buttonsScript.actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
                        
                        break;
                    case 1:
                        buttonsScript.actualChip = Instantiate(buttonsScript.crossGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                        buttonsScript.actualChip.SetActive(true);
                        buttonsScript.actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
                        
                        break;
                }
            }
        }
    }
    
    public void EnableButtons()
    {
        for(int i = 0; i < buttonsReference.Length; i++){
            buttonsReference[i].interactable = true;
        }
    }
    
    public void DisableButtons(){
        for(int i = 0; i < buttonsReference.Length; i++){
            buttonsReference[i].interactable = false;
        }
    }
}
