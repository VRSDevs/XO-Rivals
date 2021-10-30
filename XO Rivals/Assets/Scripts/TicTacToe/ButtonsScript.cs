using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
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
    private PlayerInfo localPlayer;

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
            gameState.filledPositions = new int[3,3];
            for(int i = 0; i < gameState.filledPositions.GetLength(0); i++){
                for(int j = 0; j < gameState.filledPositions.GetLength(1); j++){
                    gameState.filledPositions[i,j] = 3;
                }
            }
            
            //Start variables
            gameState.numFilled = 0;
            gameState.chips = new List<GameObject>(); 
            
            //El minijuego es elegido automaticamente
            gameState.miniGameChosen = Random.Range(0,2);
            gameState.turnMoment = 0;
        }

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();
    }
    
    public void Start(){

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
        if(gameState.filledPositions[col,row] == 3){
            
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
                gameState.chips.Add(actualChip);
                for(int i = 0; i < gameState.chips.Count; i++)
                    gameState.chips[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();

                //Save pos
                gameState.filledPositions[col,row] = 0;

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
                gameState.chips.Add(actualChip);
                for(int i = 0; i < gameState.chips.Count; i++)
                    gameState.chips[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();

                //Save pos
                gameState.filledPositions[col,row] = 1;

                //Disable input because its not your turn
                screenManager.DisableButtons();
            }

            //Add one to count
            gameState.numFilled++;
        
            //Check victory
            CheckVictory();

            //Table full (draw)
            if(gameState.numFilled == 9){
                Debug.Log("Draw");
            }
        }else{
            Debug.Log("Tile not empty");
        }
    }

    private void PlayMinigame(){

        miniWin = false;
        switch(gameState.miniGameChosen){
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
            gameState.filledPositions[col,row] = (gameState.WhosTurn == gameState.PlayerInfoO ? 0: 1);
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

            if(win){
                //Call endgame
                if(gameState.filledPositions[col,row] == 0)
                    /*
                    //Go to Victory / Lose screen depending on who you are (must be a variable stored in the match within the player ID) 
                    */
                    Debug.Log("CIRCLE WIN");
                else
                    Debug.Log("CROSS WINS");
            break;
            }

            i++;
        }while(i < 8);
    }   

    bool TestCol(int col){
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if(gameState.filledPositions[col,j] != 3){
            type = gameState.filledPositions[col,j];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(gameState.filledPositions[col,j] != type){
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
        if(gameState.filledPositions[j,row] != 3){
            type = gameState.filledPositions[j,row];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(gameState.filledPositions[j,row] != type){
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
            if(gameState.filledPositions[diag,j] != 3){
                type = gameState.filledPositions[diag,j];
                j++;
                diag++;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(gameState.filledPositions[diag,j] != type){
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
            if(gameState.filledPositions[diag,j] != 3){
                type = gameState.filledPositions[diag,j];
                j++;
                diag--;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(gameState.filledPositions[diag,j] != type){
                    return false;
                }
                diag--;
                j++;
            }while(j < 3);

            return true;
        }
    }
}

    
