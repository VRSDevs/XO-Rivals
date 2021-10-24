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
   
    //Set turn to whoever starts (variable managed from other scene)
    public static int turn = 0;

    //Control of miniGameChoosing
    [SerializeField] private GameObject miniGameChoosing;
    [SerializeField] private GameObject ticTacScreen;
    public static int minigame = -1;

    void Start(){

        if(turn == 0){
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }        
    }

    public void UpdateTurn(int num){

        turn = num;
        
        //Display turn in screen
        if(turn == 0){            
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
        minigame = n;
        miniGameChoosing.SetActive(false);
        ticTacScreen.SetActive(true);
    }
}
