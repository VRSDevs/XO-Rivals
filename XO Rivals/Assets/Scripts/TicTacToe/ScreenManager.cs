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

        if(buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn == buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName){
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }
    }

    public void UpdateTurn(){
        
        Debug.Log("X: " + buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName);
        Debug.Log("O: " + buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName);
        
        if(buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName == buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn)
            buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName;
        else
            buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName;
        
        buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].TurnMoment = 0;
        //Display turn in screen
        if(buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn == buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName){            
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }

        if (buttonsScript.miniWin)
        {
            buttonsScript.gameState._networkCommunications.SendMatchInfo("OppWon");
        }
        else
        {
            buttonsScript.gameState._networkCommunications.SendMatchInfo("OppLost");
        }
    }
    
    public IEnumerator txtTimer(string txt){
        screenTxt.enabled = true;
        screenTxt.text = txt;
        yield return new WaitForSeconds(totalTime);
        screenTxt.enabled = false;
        buttonsScript.thisMatch.TurnMoment = 4;
    }

    public void MinigameSelectionActivation(){
        //ticTacScreen.SetActive(false);
        //Hide chips
        //for(int i = 0; i < buttonsScript.thisMatch.Chips.Count; i++){
        //    buttonsScript.ChipsList[i].SetActive(false);
        //}
        miniGameChoosing.SetActive(true);
    }    

    public void MinigameSelection(int n){
        buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen = n;
        miniGameChoosing.SetActive(false);
        ticTacScreen.SetActive(true);
        
        //Update turn data and send it to opponent
        UpdateTurn();

        ////Unhide chips
        //for(int i = 0; i < buttonsScript.thisMatch.Chips.Count; i++){
        //    buttonsScript.thisMatch.Chips[i].SetActive(true);
        //}
        //Disable interaction with tictac
        DisableButtons();
    }

    public void UpdateBoard(int col, int row, string tileName)
    {
        /*
        GameObject tile = GameObject.Find(tileName);
        
        switch (buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].FilledPositions[col, row])
        {
            case 0:
                buttonsScript.thisMatch.ActualChip = Instantiate(buttonsScript.circleGO, tile.transform.position, Quaternion.identity);
                buttonsScript.thisMatch.ActualChip.SetActive(true);
                buttonsScript.thisMatch.ActualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
                        
                break;
            case 1:
                buttonsScript.thisMatch.ActualChip = Instantiate(buttonsScript.crossGO, tile.transform.position, Quaternion.identity);
                buttonsScript.thisMatch.ActualChip.SetActive(true);
                buttonsScript.thisMatch.ActualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
                        
                break;
        }
        
        buttonsScript.gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].Chips.Add(buttonsScript.thisMatch.ActualChip);

        */
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
