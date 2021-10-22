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
    private List<GameObject> chipList;

    //Variables for victory
    int col, row, filled;

    //Minigame chosen
    private int miniChosen;
    private int opponentMinigame;

    //Minigame won
    private bool miniWin;

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

        //Fill array
        positions = new int[3,3];
        for(int i = 0; i < positions.GetLength(0); i++){
            for(int j = 0; j < positions.GetLength(1); j++){
                positions[i,j] = 3;
            }
        }

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();

        //Start variables
        filled = 0;
        chipList = new List<GameObject>();

        //Return opponent chosen minigame (if exists)
        if(PlayerPrefs.HasKey("minigameChosen"))
            miniChosen = PlayerPrefs.GetInt("minigameChosen");
    }
    
    public void PlaceTile(int pos){

        //Get row and column
        col = pos % 3;
        row = pos / 3;
        
        //Check if position is already filled
        if(positions[col,row] == 3){
            
            //Places a sprite or another depending on turn
            if(ScreenManager.turn == 0){
                
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
                if(miniWin){
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
                miniChosen = ScreenManager.minigame;
                
                //Unihide chips
                for(int i = 0; i < chipList.Count; i++)
                    chipList[i].SetActive(true);

                //Change turn and save pos
                positions[col,row] = ScreenManager.turn;
                screenManager.UpdateTurn(1);
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

                //Go to selectMinigame for opponent
                Time.timeScale = 0f;
                screenManager.MinigameSelectionActivation();
                miniChosen = ScreenManager.minigame;
                Time.timeScale = 1f;
                
                //Change turn and save pos
                positions[col,row] = ScreenManager.turn;
                screenManager.UpdateTurn(0);
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

    
