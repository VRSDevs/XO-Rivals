using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScreenManagerAI : MonoBehaviour
{
    //Turn Text
    [SerializeField] private Text screenTxt;

    //Timer variable
    private float totalTime = 2f;

    //Control of miniGameChoosing
    [SerializeField] private GameObject miniGameChoosing;
    [SerializeField] private Transform[] miniGamePlaces;
    [SerializeField] private Button[] miniGameImages;
    [SerializeField] private GameObject ticTacScreen;
    [SerializeField] private GameObject instructions;

    [SerializeField] private GameObject instructionsCocinitas;
    [SerializeField] private GameObject instructionsCocinitasLore;
    [SerializeField] private GameObject instructionsCocinitasInstr;

    [SerializeField] private GameObject instructionsLaberinto;
    [SerializeField] private GameObject instructionsLaberintoLore;
    [SerializeField] private GameObject instructionsLaberintoInstr;

    [SerializeField] private GameObject instructionsPistolero;
    [SerializeField] private GameObject instructionsPistoleroLore;
    [SerializeField] private GameObject instructionsPistoleroInstr;

    [SerializeField] private GameObject instructionsPlataformas;
    [SerializeField] private GameObject instructionsPlataformasLore;
    [SerializeField] private GameObject instructionsPlataformasInstr;

    [SerializeField] private GameObject instructionsCarnival;
    [SerializeField] private GameObject instructionsCarnivalLore;
    [SerializeField] private GameObject instructionsCarnivalInstr;

    [SerializeField] private ButtonScriptAI buttonsScript;
    public static int minigame = -1;

    //Reference to all butons
    [SerializeField] private Button[] buttonsReference;
    
    private TicTacAI ticTacAI;

    private void Start() {
        ticTacAI = FindObjectOfType<TicTacAI>();
    }

    public void UpdateTurn(){
        
        
        if(buttonsScript.thisMatch.PlayerOName == buttonsScript.thisMatch.WhosTurn)
            buttonsScript.thisMatch.WhosTurn = buttonsScript.thisMatch.PlayerXName;
        else
            buttonsScript.thisMatch.WhosTurn = buttonsScript.thisMatch.PlayerOName;

        buttonsScript.thisMatch.TurnMoment = 0;

        //Display turn in screen
        if(buttonsScript.thisMatch.WhosTurn == buttonsScript.thisMatch.PlayerOName){            
            StartCoroutine(txtTimer("Turno de O"));
        }else{
            StartCoroutine(txtTimer("Turno de X"));
        }
        
        if(buttonsScript.thisMatch.WhosTurn == buttonsScript.thisMatch.PlayerXName)
            ticTacAI.UpdateAI();
        else
            buttonsScript.startGame(); 
    }
    
    public IEnumerator txtTimer(string txt){
        screenTxt.enabled = true;
        screenTxt.text = txt;
        yield return new WaitForSeconds(totalTime);
        screenTxt.enabled = false;
        buttonsScript.thisMatch.TurnMoment = 4;
    }

    public void MinigameSelectionActivation(){

        miniGameChoosing.SetActive(true);

        //Randomly choose minigames
        int located = 0;
        int num;
        List<int> selected = new List<int>();
        while(located < 3){
            num = Random.Range(0,5);
            if(!selected.Contains(num)){
                selected.Add(num);
                located++;
            }
        }

        //Locate each minigame in its place
        for(int i = 0; i < selected.Count; i++){
            
            miniGameImages[selected[i]].gameObject.SetActive(true);
            miniGameImages[selected[i]].transform.position = miniGamePlaces[i].transform.position;
        }
    } 
   

    public void MinigameSelection(int n){

        //buttonsScript.thisMatch.MiniGameChosen = n;
        buttonsScript.thisMatch.MiniGameChosen = 0;
        miniGameChoosing.SetActive(false);
        ticTacScreen.SetActive(true);

        //Disable interaction with tictac
        DisableButtons();
        
        //Update turn data and send it to opponent
        UpdateTurn();
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


    public void showInstruction(int i)
    {
        instructions.SetActive(true);

        switch (i)
        {
            case 0://Pistolero
                instructionsPistolero.SetActive(true);
                instructionsPistoleroInstr.SetActive(true);
                break;

            case 1://Comida
                instructionsCocinitas.SetActive(true);
                instructionsCocinitasInstr.SetActive(true);

                break;

            case 2://Platform
                instructionsPlataformas.SetActive(true);
                instructionsPlataformasInstr.SetActive(true);

                break;
            case 3://Laberinto
                instructionsLaberinto.SetActive(true);
                instructionsLaberintoInstr.SetActive(true);
                break;
            case 4://Feria
                instructionsCarnival.SetActive(true);
                instructionsCarnivalInstr.SetActive(true);
                break;
        }
    }

    public void unshowInstructions()
    {

        instructionsPistolero.SetActive(false);
        instructionsPistoleroInstr.SetActive(false);
        instructionsPistoleroLore.SetActive(false);


        instructionsPlataformas.SetActive(false);
        instructionsPlataformasInstr.SetActive(false);
        instructionsPlataformasLore.SetActive(false);

        instructionsCocinitas.SetActive(false);
        instructionsCocinitasInstr.SetActive(false);
        instructionsCocinitasLore.SetActive(false);


        instructionsLaberinto.SetActive(false);
        instructionsLaberintoInstr.SetActive(false);
        instructionsLaberintoLore.SetActive(false);

        instructionsCarnival.SetActive(false);
        instructionsCarnivalInstr.SetActive(false);
        instructionsCarnivalLore.SetActive(false);

        instructions.SetActive(false);

    }


    public void goInstrLore()
    {
        int i = buttonsScript.thisMatch.MiniGameChosen;

        switch (i)
        {
            case 0://Pistolero

                instructionsPistoleroInstr.SetActive(false);
                instructionsPistoleroLore.SetActive(true);
                break;

            case 1://Comida

                instructionsCocinitasInstr.SetActive(false);
                instructionsCocinitasLore.SetActive(true);

                break;

            case 2://Platform

                instructionsPlataformasInstr.SetActive(false);
                instructionsPlataformasLore.SetActive(true);

                break;
            case 3://Laberinto

                instructionsLaberintoInstr.SetActive(false);
                instructionsLaberintoLore.SetActive(true);

                break;
            case 4://Feria

                instructionsCarnivalInstr.SetActive(false);
                instructionsCarnivalLore.SetActive(true);

                break;
        }
    }
    
    public void goInstrtext()
    {
        int i = buttonsScript.thisMatch.MiniGameChosen;

        switch (i)
        {
            case 0://Pistolero

                instructionsPistoleroInstr.SetActive(true);
                instructionsPistoleroLore.SetActive(false);
                break;

            case 1://Comida

                instructionsCocinitasInstr.SetActive(true);
                instructionsCocinitasLore.SetActive(false);

                break;

            case 2://Platform

                instructionsPlataformasInstr.SetActive(true);
                instructionsPlataformasLore.SetActive(false);

                break;
            case 3://Laberinto

                instructionsLaberintoInstr.SetActive(true);
                instructionsLaberintoLore.SetActive(false);

                break;
            case 4://Carnival

                instructionsCarnivalInstr.SetActive(true);
                instructionsCarnivalLore.SetActive(false);

                break;
        }
    }
}
