using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Random = UnityEngine.Random;
using PlayFab;

public class ButtonsScript : MonoBehaviour
{
    #region Variables

    //Circle and Cross
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite cross;
    public GameObject circleGO;
    public GameObject crossGO;

    public List<Transform> botonesCuadricula;

    //Screen manager
    private ScreenManager screenManager;

    //Array of positions
    public string SelectedTile;

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

    public List<GameObject> ChipsList;

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
        Debug.Log(thisMatch.FilledPositions[0,0] + " " + thisMatch.FilledPositions[0,1] + " " + thisMatch.FilledPositions[0,2] + "\n" 
        + thisMatch.FilledPositions[1,0] + " " + thisMatch.FilledPositions[1,1] + " " + thisMatch.FilledPositions[1,2] + "\n" 
        + thisMatch.FilledPositions[2,0] + " " + thisMatch.FilledPositions[2,1] + " " + thisMatch.FilledPositions[2,2]);
        Debug.Log("Minigame chosen: " + thisMatch.MiniGameChosen);

        startGame();


        colocarFichas();

        //CheckVictory();

        //SI VIENES DE UN MINIJUEGO SE HACE START Y SE ELIGE MINIJUEGO
        if (thisMatch.TurnMoment == 2)
        {
            Debug.Log("AAAAAAAA");

            miniWin = (PlayerPrefs.GetInt("minigameWin") == 1);
            Debug.Log(miniWin);

            if (miniWin)
            {
                GameObject actual;
                if (thisMatch.ActualChipTeam == "cross")
                {
                    //Save position
                    thisMatch.FilledPositions[thisMatch.ActualChip % 3, thisMatch.ActualChip / 3] = 1;
                    //Paint tile completely

                    actual = Instantiate(crossGO, botonesCuadricula[thisMatch.ActualChip].position, Quaternion.identity);
                    actual.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    actual.SetActive(true);

                    //Add chip to list
                    ChipsList.Add(actual);
                    //Add one to filled count
                    thisMatch.NumFilled++;
                }
                else if (thisMatch.ActualChipTeam == "circle")
                {
                    //Save position
                    thisMatch.FilledPositions[thisMatch.ActualChip % 3, thisMatch.ActualChip / 3] = 0;
                    //Paint tile completely

                    actual = Instantiate(circleGO, botonesCuadricula[thisMatch.ActualChip].position, Quaternion.identity);
                    actual.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    actual.SetActive(true);

                    //Add chip to list
                    ChipsList.Add(actual);
                    //Add one to filled count
                    thisMatch.NumFilled++;
                }
            }//Fin win

            Debug.Log("SE HACE MINIGAME SELECTION");
            screenManager.MinigameSelectionActivation();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void startGame()
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
                //PlayMinigame();
            }else if(thisMatch.TurnMoment == 3){
                //Go to check victory
                CheckVictory();
            }else if(thisMatch.TurnMoment == 4){
                //Go to choose minigame
                //screenManager.MinigameSelectionActivation();
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
            {
                thisMatch.ActualChip = pos;
                thisMatch.ActualChipTeam = "circle";
            }

            else
            {
                thisMatch.ActualChip = pos;
                thisMatch.ActualChipTeam = "cross";
            }
                

            //thisMatch.ActualChip.SetActive(true);
            //thisMatch.ActualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
            thisMatch.TurnMoment = 1;

            //Go to minigame
            //PlayMinigame();

            screenManager.showInstruction(thisMatch.MiniGameChosen);

        }else{
            Debug.Log("Tile not empty");
        }
    }

    public void PlayMinigame(){

        //If turnMoment equals 1, have to play minigame, else, you have already played it
        if(thisMatch.TurnMoment == 1){
            miniWin = false;
            PlayerPrefs.SetInt("minigameWin", 0);
            switch(thisMatch.MiniGameChosen){
                case 0:
                    SceneManager.LoadScene("Pistolero");
                    //SceneManager.LoadScene("Pistolero", LoadSceneMode.Additive);
                break;

                case 1:
                    SceneManager.LoadScene("MinijuegoComida");
                    //SceneManager.LoadScene("MinijuegoComida", LoadSceneMode.Additive);
                break;

                case 2:
                    SceneManager.LoadScene("PlatformMinigame");
                    //SceneManager.LoadScene("PlatformMinigame", LoadSceneMode.Additive);
                break;
                case 3:
                    SceneManager.LoadScene("Fantasmas3D");
                    //SceneManager.LoadScene("PlatformMinigame", LoadSceneMode.Additive);
                    break;
            }
        }

        /*
        else if(thisMatch.TurnMoment == 2){

            Debug.Log("NOJUEGO");

            //Check minigame win
            miniWin = (PlayerPrefs.GetInt("minigameWin") == 1);
            if(miniWin == true){

                //Save position
                thisMatch.FilledPositions[col,row] = (thisMatch.WhosTurn == thisMatch.PlayerOName ? 0: 1);
                //Paint tile completely
                thisMatch.ActualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
                //Add chip to list
                thisMatch.Chips.Add(thisMatch.ActualChip);

                //Add one to filled count
                thisMatch.NumFilled++;

                //Disable input because its not your turn
                screenManager.DisableButtons();

                //If someone won or draw, go to next scene. Else, choose minigame for opponent
                thisMatch.TurnMoment = 3;
                CheckVictory();
            }else{

                //No need to checkVictory because no tile has been set
                Destroy(thisMatch.ActualChip);
                StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));
                screenManager.MinigameSelectionActivation();
            }
        }
        */



    }
    
    public void colocarFichas()
    {

        //SE COLOCAN LAS FICHAS EXISTENTES
        for (int i = 0; i < 9; i++)
        {

            if (thisMatch.FilledPositions[i % 3, i / 3] == 0)//Circle
            {
                GameObject actual;
                actual = Instantiate(circleGO, botonesCuadricula[i].position, Quaternion.identity);
                actual.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                actual.SetActive(true);
            }

            if (thisMatch.FilledPositions[i % 3, i / 3] == 1)//Cross
            {
                GameObject actual;
                actual = Instantiate(crossGO, botonesCuadricula[i].position, Quaternion.identity);
                actual.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                actual.SetActive(true);
            }

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
                        localPlayer.Level += 0.75f;
                        UpdateLevel();
                        gameState._networkCommunications.SendEndMatchInfo("win", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName);
                    }
                    else
                    {
                        localPlayer.Level += 0.35f;
                        UpdateLevel();
                        gameState._networkCommunications.SendEndMatchInfo("defeat", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName);
                    }
                }
                else
                {
                    Debug.Log("CROSS WINS");

                    if (localPlayer.Name == thisMatch.PlayerXName)
                    {
                        localPlayer.Level += 0.75f;
                        UpdateLevel();
                        gameState._networkCommunications.SendEndMatchInfo("win", gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName);
                    }
                    else
                    {
                        localPlayer.Level += 0.35f;
                        UpdateLevel();
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
        //screenManager.MinigameSelectionActivation();
    }

    private void UpdateLevel(){

        //Upload lifes to server
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                Data = new Dictionary<string, string>() {
                    {"Level", localPlayer.Level.ToString()}}
            },
            result => Debug.Log("Successfully updated user level"),
            error => {
                Debug.Log("Got error setting user level");
            }
        );
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

    
