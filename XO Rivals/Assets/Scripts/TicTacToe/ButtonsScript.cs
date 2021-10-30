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
    private int[,] positions;
    public static List<GameObject> chipList;

    //Variables for victory
    int col, row, filled;

    //Minigame chosen
    public static int miniChosen;
    private int opponentMinigame;

    //Minigame won
    private bool miniWin;

<<<<<<< Updated upstream
=======
    //Match controller
    public GameManager gameState;

    //Local player
    private PlayerInfo localPlayer;

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        //Fill array
        positions = new int[3,3];
        for(int i = 0; i < positions.GetLength(0); i++){
            for(int j = 0; j < positions.GetLength(1); j++){
                positions[i,j] = 3;
=======
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
>>>>>>> Stashed changes
            }
        }

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();
<<<<<<< Updated upstream
=======
    }
    
    public void Start(){

        //If its your turn, play, if its not, only can see
        if(gameState.WhosTurn == localPlayer){
>>>>>>> Stashed changes

        //Start variables
        filled = 0;
        chipList = new List<GameObject>();

<<<<<<< Updated upstream
        //Return opponent chosen minigame (if exists)
        if(PlayerPrefs.HasKey("minigameChosen"))
            miniChosen = PlayerPrefs.GetInt("minigameChosen");
=======
            }else if(gameState.turnMoment == 2){
                //Go to choose minigame
                screenManager.MinigameSelectionActivation();
            }
        }else{
            //Disable interaction with tictac cause its not your turn
            screenManager.DisableButtons();
        }
>>>>>>> Stashed changes
    }
    
    public void PlaceTile(int pos){

        //Get row and column
        col = pos % 3;
        row = pos / 3;
        
        //Check if position is already filled
        if(positions[col,row] == 3){
            
            //Places a sprite or another depending on turn
            if(gameState.PlayerInfoO == localPlayer){
                
                GameObject newCircle = Instantiate(circleGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                newCircle.SetActive(true);
                newCircle.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);

                /*//Go to minigame
                public bool miniWin = false; //This variable will be written from minigames
                switch(miniChosen){
                    case 0:
                        SceneManager.LoadScene("Pistolero", LoadSceneMode.Additive);
                    break;

                    case 1:
                        SceneManager.LoadScene("Minijuego1", LoadSceneMode.Additive);
                    break;

                    case 2:
                        SceneManager.LoadScene("Minijuego2", LoadSceneMode.Additive);
                    break;

                }

                //Check minigame win
                miniWin = PlayerPrefs.GetInt("minigameWin");
                if(miniWin == 1){
                    //Save position
                    positions[col,row] = ScreenManager.turn;
                    //Paint tile completely
                    newCircle.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
                }else{
                    destroy(newCircle);
                    StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));
                }

                */

                //Add chip to list to hide
                chipList.Add(newCircle);
                for(int i = 0; i < chipList.Count; i++)
                    chipList[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();

                //Save pos
<<<<<<< Updated upstream
                positions[col,row] = ScreenManager.turn;
=======
                gameState.filledPositions[col,row] = 0;
>>>>>>> Stashed changes

                //Disable input because its not your turn
            }else{

                GameObject newCross = Instantiate(crossGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                newCross.SetActive(true);
                newCross.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.35f);

                /*//Go to minigame
                public bool miniWin = false; //This variable will be written from minigames
                switch(miniChosen){
                    case 0:
                        SceneManager.LoadScene("Pistolero", LoadSceneMode.Additive);
                    break;

                    case 1:
                        SceneManager.LoadScene("Minijuego1", LoadSceneMode.Additive);
                    break;

                    case 2:
                        SceneManager.LoadScene("Minijuego2", LoadSceneMode.Additive);
                    break;

                }

                //Check minigame win
                if(miniWin){
                    //Save position
                    positions[col,row] = ScreenManager.turn;
                    //Paint tile completely
                    newCross.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
                }else{
                    destroy(newCross);
                    StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));
                }

                */
                //Add chip to list to hide
                chipList.Add(newCross);
                for(int i = 0; i < chipList.Count; i++)
                    chipList[i].SetActive(false);

                //Go to selectMinigame for opponent
                screenManager.MinigameSelectionActivation();
                
                //Save pos
                positions[col,row] = ScreenManager.turn;

                //Save pos
                gameState.filledPositions[col,row] = 1;

                //Disable input because its not your turn
            }

            //Add one to count
            filled++;
        
            //Check victory
            CheckVictory();

            //Table full (draw)
            if(filled == 9){
                Debug.Log("Draw");
            }
        }else{
            Debug.Log("Tile not empty");
        }
    }

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
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
                if(positions[col,row] == 0)
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
        if(positions[col,j] != 3){
            type = positions[col,j];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(positions[col,j] != type){
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
        if(positions[j,row] != 3){
            type = positions[j,row];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(positions[j,row] != type){
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
            if(positions[diag,j] != 3){
                type = positions[diag,j];
                j++;
                diag++;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(positions[diag,j] != type){
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
            if(positions[diag,j] != 3){
                type = positions[diag,j];
                j++;
                diag--;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(positions[diag,j] != type){
                    return false;
                }
                diag--;
                j++;
            }while(j < 3);

            return true;
        }
    }
}

    
