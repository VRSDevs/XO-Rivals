using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonsScript : MonoBehaviour
{
    #region Variables

    //Circle and Cross
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite cross;
    private GameObject circleGO;
    private GameObject crossGO;

    //Screen manager
    private ScreenManager screenManager;

    //Array of positions
    private GameObject actualChip;

    //Variables for victory
    int col, row;

    //Minigame chosen
    private int opponentMinigame;

    //Minigame won
    private bool miniWin;

    //Match controller
    public GameManager gameState;

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
        localPlayer = FindObjectOfType<PlayerInfo>();

        //If its a new match, there is no playerX
        if(gameState.PlayerInfoX == null){ 

            //Fill array
            gameState.FilledPositions = new int[3,3];
            for(int i = 0; i < gameState.FilledPositions.GetLength(0); i++){
                for(int j = 0; j < gameState.FilledPositions.GetLength(1); j++){
                    gameState.FilledPositions[i,j] = 3;
                }
            }
            
            //Start variables
            gameState.NumFilled = 0;
            gameState.Chips = new List<GameObject>(); 
            
            //El minijuego es elegido automaticamente
            gameState.MiniGameChosen = Random.Range(0,2);
            gameState.turnMoment = 0;
        }

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();
    }
    
    public void Start(){
        
        UpdateTurn();
    }

    #endregion

    #region MainMethods

    /// <summary>
    /// 
    /// </summary>
    public void UpdateTurn()
    {
        Debug.Log(gameState.WhosTurn);
        Debug.Log(localPlayer);

        //If its your turn, play, if its not, only can see
        if(gameState.WhosTurn == localPlayer){

            //Depending of turn moment, player will encounter a "different scene"
            //if(turnInstant == 0){
            //Nothing happens
            /*}else*/
            if(gameState.turnMoment == 1){
                //Go directly to minigame
                PlayMinigame();
            }else if(gameState.turnMoment == 2){
                //Go to choose minigame
                screenManager.MinigameSelectionActivation();
            }
        }else{
            //Disable interaction with tictac cause its not your turn
            screenManager.DisableButtons();
        }
    }

    public void PlaceTile(int pos){

        //Get row and column
        col = pos % 3;
        row = pos / 3;
        
        //Check if position is already filled
        if(gameState.FilledPositions[col,row] == 3){
            
            //Places a sprite or another depending on turn
            if(gameState.PlayerInfoO == localPlayer){
                
                //Place chip
                actualChip = Instantiate(circleGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                actualChip.SetActive(true);
                actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);
                gameState.turnMoment = 1;

                //Go to minigame
                PlayMinigame();

                //Add chip to list to hide
                gameState.Chips.Add(actualChip);
                for(int i = 0; i < gameState.Chips.Count; i++)
                    gameState.Chips[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();

                //Save pos
                gameState.FilledPositions[col,row] = 0;

                //Disable input because its not your turn
                screenManager.DisableButtons();

            }else{

                //Place chip
                actualChip = Instantiate(crossGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                actualChip.SetActive(true);
                actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);

                //Go to minigame
                PlayMinigame();

                //Add chip to list to hide
                gameState.Chips.Add(actualChip);
                for(int i = 0; i < gameState.Chips.Count; i++)
                    gameState.Chips[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();

                //Save pos
                gameState.FilledPositions[col,row] = 1;

                //Disable input because its not your turn
                screenManager.DisableButtons();
            }

            //Add one to count
            gameState.NumFilled++;
        
            //Check victory
            CheckVictory();

            //Table full (draw)
            if(gameState.NumFilled == 9){
                Debug.Log("Draw");
            }
        }else{
            Debug.Log("Tile not empty");
        }
    }

    private void PlayMinigame(){

        miniWin = false;
        switch(gameState.MiniGameChosen){
            case 0:
                SceneManager.LoadScene("Pistolero", LoadSceneMode.Additive);
            break;

            case 1:
                SceneManager.LoadScene("MinijuegoComida", LoadSceneMode.Additive);
            break;

            case 2:
                SceneManager.LoadScene("2D Platform", LoadSceneMode.Additive);
            break;
        }

        //Check minigame win
        miniWin = (PlayerPrefs.GetInt("minigameWin") == 1);
        if(miniWin == true){
            //Save position
            gameState.FilledPositions[col,row] = (gameState.WhosTurn == gameState.PlayerInfoO ? 0: 1);
            //Paint tile completely
            actualChip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
        }else{
            Destroy(actualChip);
            StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));
        }
        gameState.turnMoment = 2;
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
                PlayerInfo localPlayer = FindObjectOfType<PlayerInfo>();

                gameState.IsPlaying = false;
                
                //Call endgame
                if (gameState.FilledPositions[col, row] == 0)
                {
                    Debug.Log("CIRCLE WIN");
                    
                    if (localPlayer.Name == gameState.PlayerInfoO.Name)
                    {
                        FindObjectOfType<EndGameScript>().ShowMatchVictory();
                    }
                    else
                    {
                        FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                    }
                }
                else
                {
                    Debug.Log("CROSS WINS");

                    if (localPlayer.Name == gameState.PlayerInfoX.Name)
                    {
                        FindObjectOfType<EndGameScript>().ShowMatchVictory();
                    }
                    else
                    {
                        FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                    }
                }
                break;
            }

            i++;
        }while(i < 8);
    }

    #endregion

    #region TestMethods

    bool TestCol(int col){
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if(gameState.FilledPositions[col,j] != 3){
            type = gameState.FilledPositions[col,j];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(gameState.FilledPositions[col,j] != type){
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
        if(gameState.FilledPositions[j,row] != 3){
            type = gameState.FilledPositions[j,row];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(gameState.FilledPositions[j,row] != type){
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
            if(gameState.FilledPositions[diag,j] != 3){
                type = gameState.FilledPositions[diag,j];
                j++;
                diag++;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(gameState.FilledPositions[diag,j] != type){
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
            if(gameState.FilledPositions[diag,j] != 3){
                type = gameState.FilledPositions[diag,j];
                j++;
                diag--;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(gameState.FilledPositions[diag,j] != type){
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

    
