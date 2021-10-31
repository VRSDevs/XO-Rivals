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

    public void UpdateTurn(int miniJuego){

        buttonsScript.actualizarTurno(miniJuego);

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

        Debug.Log("MINIJUEGO SELECCIONADO PARA MI");


        //Disable interaction with tictac
        DisableButtons();
        UpdateTurn(n);

        
    }

    public void DisableButtons(){
        for(int i = 0; i < buttonsReference.Length; i++){
            buttonsReference[i].interactable = false;
        }
    }

    public void EnableButtons()
    {
        for (int i = 0; i < buttonsReference.Length; i++)
        {
            buttonsReference[i].interactable = true;
        }
    }

}
 