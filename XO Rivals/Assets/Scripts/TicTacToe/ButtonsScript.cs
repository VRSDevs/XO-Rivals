﻿using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Random = UnityEngine.Random;

public class ButtonsScript : MonoBehaviour
{
    #region Variables

    //Circle and Cross
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite cross;
    public GameObject circleGO;
    public GameObject crossGO;

    //Screen manager
    private ScreenManager screenManager;

    //Array of positions
    public string SelectedTile;
    public GameObject actualChip;

    //Variables for victory
    public int col, row;

    //Minigame chosen
    private int opponentMinigame;

    //Minigame won
    public bool miniWin;

    //Gamemanager controller
    public GameManager gameState;

    //Match information
    public Match thisMatch;

    //Local player
    public PlayerInfo localPlayer;

    #endregion

    #region UnityCB

    private void Awake() {
        //Create the circle
        circleGO = new GameObject();
        SpriteRenderer circleRenderer = circleGO.AddComponent<SpriteRenderer>();
        circleRenderer.sprite = circle;
        circleGO.SetActive(false);

        //Create the cross
        crossGO = new GameObject();
        SpriteRenderer crossRenderer = crossGO.AddComponent<SpriteRenderer>();
        crossRenderer.sprite = cross;
        crossGO.SetActive(false);

        gameState = FindObjectOfType<GameManager>();
        thisMatch = gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        
        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();
    }
    
    public void Start(){
        
        Debug.Log("Player O name: " + thisMatch.PlayerOName);
        Debug.Log("Player X name: " + thisMatch.PlayerXName);
        Debug.Log("Turn: " + thisMatch.WhosTurn);
        Debug.Log("Turn moment: " + thisMatch.TurnMoment);
        Debug.Log("Numfilled: " + thisMatch.NumFilled);
        Debug.Log("Board: ");
        for(int i = 0; i < 3; i++){
            Debug.Log("[" + i);
            for(int j = 0; j < 3; j++){
                Debug.Log(j + "] = " + thisMatch.FilledPositions[i,j]);
            }
        }
        Debug.Log("Minigame chosen: " + thisMatch.MiniGameChosen);

        UpdateTurn();
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateTurn()
    {
        //If its your turn, play, if its not, only can see
        if(thisMatch.WhosTurn == localPlayer.Name){

            //Depending of turn moment, player will encounter a "different scene"
            if (thisMatch.TurnMoment == 0)
            {
                screenManager.EnableButtons();
            }
            else if(thisMatch.TurnMoment == 1 || thisMatch.TurnMoment == 2){
                //Go directly to minigame
                PlayMinigame();
            }else if(thisMatch.TurnMoment == 3){
                //Go to check victory
                CheckVictory();
            }else if(thisMatch.TurnMoment == 4){
                //Go to choose minigame
                screenManager.MinigameSelectionActivation();
            }
        }else{
            //Disable interaction with tictac cause its not your turn
            screenManager.DisableButtons();
        }
    }

    #endregion

    #region MainMethods
    
    public void PlaceTile(int pos){

        //Get row and column
        col = pos % 3;
        row = pos / 3;
        
        //Check if position is already filled
        if(thisMatch.FilledPositions[col,row] == 3){
            
            //Places a sprite or another depending on player
            GameObject tile = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            SelectedTile = tile.name;
            
            //Set chip to player type
            if(thisMatch.PlayerOName == localPlayer.Name)
                actualChip = Instantiate(circleGO, tile.transform.position, Quaternion.identity);
            else
                actualChip = Instantiate(crossGO, tile.transform.position, Quaternion.identity);

            actualChip.SetActive(true);
            actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
            thisMatch.TurnMoment = 1;

            //Go to minigame
            PlayMinigame();
        }else{
            Debug.Log("Tile not empty");
        }
    }

    private void PlayMinigame(){

        //If turnMoment equals 1, have to play minigame, else, you have already played it
        if(thisMatch.TurnMoment == 1){
            miniWin = false;
            PlayerPrefs.SetInt("minigameWin", 0);
            switch(thisMatch.MiniGameChosen){
                case 0:
                    SceneManager.LoadScene("Pistolero");
                break;

                case 1:
                    SceneManager.LoadScene("MinijuegoComida");
                break;

                case 2:
                    SceneManager.LoadScene("2D Platform");
                break;
            }
        }

        //Check minigame win
        miniWin = (PlayerPrefs.GetInt("minigameWin") == 1);
        if(miniWin == true){

            //Save position
            thisMatch.FilledPositions[col,row] = (thisMatch.WhosTurn == thisMatch.PlayerOName ? 0: 1);
            //Paint tile completely
            actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
            //Add chip to list
            thisMatch.Chips.Add(actualChip);

            //Add one to filled count
            thisMatch.NumFilled++;

            //Disable input because its not your turn
            screenManager.DisableButtons();

            //If someone won or draw, go to next scene. Else, choose minigame for opponent
            thisMatch.TurnMoment = 3;
            CheckVictory();
        }else{

            //No need to checkVictory because no tile has been set
            Destroy(actualChip);
            StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));
        }
    }
    
    public void CheckVictory(){

        bool[] array = new bool[8];

        //Fill array with true every loop
        for(int w = 0; w < 8; w++){
            array[w] = true;
        }

        //If they are not equal, they are not on the main diagonal
        if(col != row){
            array[6] = false;
        }

        //If they dont add 2, they are not on the secondary diagonal
        if(col+row != 2){
            array[7] = false;
        }

        //Check column
        switch(col){
            case 0:
                array[1] = array[2] = false;
            break;

            case 1:
                array[0] = array[2] = false;
            break;

            case 2:
                array[0] = array[1] = false;
            break;
        }

        //Check row
        switch(row){
            case 0:
                array[4] = array[5] = false;
            break;

            case 1:
                array[3] = array[5] = false;
            break;

            case 2:
                array[3] = array[4] = false;
            break;
        }

        //Check every posible win
        int i = 0;
        bool win;
        do{
            if(i < 3){
                win = TestCol(i);
            }else if(i < 6){
                win = TestRow(i%3);
            }else{
                win = TestDiag(i%6);
            }

            if(win)
            {
                gameState.IsPlaying = false;
                
                //Call endgame
                if (thisMatch.FilledPositions[col, row] == 0)
                {
                    Debug.Log("CIRCLE WIN");
                    
                    if (localPlayer.Name == thisMatch.PlayerOName)
                    {
                        gameState._networkCommunications.SendEndMatchInfo("win", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName);
                    }
                    else
                    {
                        gameState._networkCommunications.SendEndMatchInfo("defeat", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName);
                    }
                }
                else
                {
                    Debug.Log("CROSS WINS");

                    if (localPlayer.Name == thisMatch.PlayerXName)
                    {
                        gameState._networkCommunications.SendEndMatchInfo("win", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName);
                    }
                    else
                    {
                        gameState._networkCommunications.SendEndMatchInfo("defeat", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName);
                    }
                }
                break;
            }
            i++;
        }while(i < 8);

        //Table full (draw)
        if(thisMatch.NumFilled == 9){
            Debug.Log("Draw");
            gameState._networkCommunications.SendEndMatchInfo("draw", "");
        }

        thisMatch.TurnMoment = 4;
        //Go to selectMinigame for opponent
        screenManager.MinigameSelectionActivation();
    }

    #endregion

    #region TestMethods

    bool TestCol(int col){
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if(thisMatch.FilledPositions[col,j] != 3){
            type = thisMatch.FilledPositions[col,j];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(thisMatch.FilledPositions[col,j] != type){
                return false;
            }
            j++;
        }while(j < 3);

        return true;
    }
    
    bool TestRow(int row){
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if(thisMatch.FilledPositions[j,row] != 3){
            type = thisMatch.FilledPositions[j,row];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(thisMatch.FilledPositions[j,row] != type){
                return false;
            }
            j++;
        }while(j < 3);

        return true;
    }

    bool TestDiag(int diag){
        int type;
        int j = 0;

        //First diagonal
        if(diag == 0){
            //Pick first tile in column if its not empty
            if(thisMatch.FilledPositions[diag,j] != 3){
                type = thisMatch.FilledPositions[diag,j];
                j++;
                diag++;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(thisMatch.FilledPositions[diag,j] != type){
                    return false;
                }
                diag++;
                j++;
            }while(j < 3);

            return true;

        //Second diagonal
        }else{
            diag++;
            //Pick first tile in column if its not empty
            if(thisMatch.FilledPositions[diag,j] != 3){
                type = thisMatch.FilledPositions[diag,j];
                j++;
                diag--;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(thisMatch.FilledPositions[diag,j] != type){
                    return false;
                }
                diag--;
                j++;
            }while(j < 3);

            return true;
        }
    }

    #endregion

}

    
