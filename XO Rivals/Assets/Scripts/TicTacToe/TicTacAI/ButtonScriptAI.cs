using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonScriptAI : MonoBehaviour
{
   #region Variables

    //Circle and Cross
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite cross;
    public GameObject circleGO;
    public GameObject crossGO;

    [SerializeField] private GameObject circleTurnRival;
    [SerializeField] private GameObject crossTurnRival;

    [SerializeField] private GameObject circleTurn;
    [SerializeField] private GameObject crossTurn;

    //Player names
    [SerializeField] private TextMeshProUGUI nameO;
    [SerializeField] private TextMeshProUGUI nameX;

    public List<Transform> botonesCuadricula;

    private ScreenManagerAI screenManager;

    public string selectedTile;

    //Minigame chosen
    private int opponentMinigame;

    //Minigame won
    public bool miniWin;

    //Match information
    public MatchAI thisMatch;
    private TicTacAI ticTacAIReference;

    //Local player
    public PlayerInfo localPlayer;

    public List<GameObject> ChipsList;

    #endregion

    #region UnityCB

    private void Awake(){

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

        thisMatch = FindObjectOfType<MatchAI>();
        DontDestroyOnLoad(thisMatch);

        //localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        DontDestroyOnLoad(localPlayer);      
        thisMatch.PlayerOName = localPlayer.Name;

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManagerAI>();
    }

    public void updateIconTurn(){

        //Activate player turn tile
        if (thisMatch.WhosTurn == thisMatch.PlayerOName)
        {
            circleTurn.SetActive(true);
            crossTurn.SetActive(false);
        }
        else
        {
            crossTurn.SetActive(true);
            circleTurn.SetActive(false);
        }
        
        colocarFichas();
    }

    public void Start(){

        startGame();

    }

    public void startGame(){

        //If its first turn
        if(thisMatch.WhosTurn == "")
            thisMatch.WhosTurn = localPlayer.Name;

        //If its your turn, play, if its not, only can see
        if(thisMatch.WhosTurn == localPlayer.Name){

            crossTurnRival.SetActive(false);
            circleTurnRival.SetActive(false);
            //Depending of turn moment, player will encounter a "different scene"
            if (thisMatch.TurnMoment == 0)
            {
                screenManager.EnableButtons();
                circleTurnRival.SetActive(false);
                crossTurnRival.SetActive(false);
            }
      
        }else{

            //Disable interaction with tictac cause its not your turn
            screenManager.DisableButtons();

            if(localPlayer.Name == thisMatch.PlayerOName){
                crossTurnRival.SetActive(true);
            }else{
                circleTurnRival.SetActive(true);
            }
        }
        
        updateIconTurn();
    }

    #endregion

    #region MainMethods

    public void PlaceTile(int pos){
        //Check if position is already filled
        if(thisMatch.FilledPositions[pos%3,pos/3] == 3){
            
            //Places a sprite or another depending on player
            GameObject tile = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            selectedTile = tile.name;
            
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
            Debug.Log("Tile not empty");Debug.Log("Tile not empty");
        }
    }

    public void PlayMinigame(){
        
        //If turnMoment equals 1, have to play minigame, else, you have already played it
        if(thisMatch.TurnMoment == 1){
            miniWin = false;
            PlayerPrefs.SetInt("minigameWin", 0);
            switch(thisMatch.MiniGameChosen){

                case 0:
                    //SceneManager.LoadScene("Pistolero_Off");
                    SceneManager.LoadScene("PruebaMini");
                break;

                case 1:
                    SceneManager.LoadScene("MinijuegoComida_Off");
                break;

                case 2:
                    SceneManager.LoadScene("PlatformMinigame_Off");
                break;
                
                case 3:
                    SceneManager.LoadScene("Fanstasmas3D_Off");
                break;

                case 4:
                    SceneManager.LoadScene("CarnivalMinigame_Off");
                break;
            }
        }
    }

    public void colocarFichas(){

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
        
        //Set name to each player
        nameO.text = thisMatch.PlayerOName;
        nameX.text = thisMatch.PlayerXName;

        //SI VIENES DE UN MINIJUEGO SE HACE START Y SE ELIGE MINIJUEGO
        if (thisMatch.TurnMoment == 2)
        {
            miniWin = (PlayerPrefs.GetInt("minigameWin") == 1);

            if (miniWin)
            {
                GameObject actual;
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

                ticTacAIReference = FindObjectOfType<TicTacAI>();
                ticTacAIReference.UpdateState();
            }//Fin win

            Debug.Log("SE HACE MINIGAME SELECTION");
            screenManager.MinigameSelectionActivation();
        }
        
        CheckVictory();
    }

    public void CheckVictory(){

        bool[] array = new bool[8];
        int col = thisMatch.ActualChip % 3;
        int row = thisMatch.ActualChip / 3;
        bool win=false;

        //Primera Fila
        if (thisMatch.FilledPositions[0 % 3, 0 / 3] != 3 && (thisMatch.FilledPositions[0 % 3, 0 / 3] == thisMatch.FilledPositions[1 % 3, 1 / 3]) && (thisMatch.FilledPositions[1 % 3,1 / 3] == thisMatch.FilledPositions[2 % 3, 2 / 3]))
        {
            win = true ;
        }
        //Segunda Fila
        if (thisMatch.FilledPositions[3 % 3, 3 / 3] != 3 && (thisMatch.FilledPositions[3 % 3, 3 / 3] == thisMatch.FilledPositions[4 % 3, 4 / 3]) && (thisMatch.FilledPositions[4 % 3, 4 / 3] == thisMatch.FilledPositions[5 % 3, 5 / 3]))
        {
            win = true;
        }
        //Tercera Fila
        if (thisMatch.FilledPositions[6 % 3, 6 / 3] != 3 && (thisMatch.FilledPositions[6 % 3, 6 / 3] == thisMatch.FilledPositions[7 % 3, 7 / 3]) && (thisMatch.FilledPositions[7 % 3, 7 / 3] == thisMatch.FilledPositions[8 % 3, 8 / 3]))
        {
            win = true;
        }
        //Primera columna
        if (thisMatch.FilledPositions[0 % 3, 0 / 3] != 3 && (thisMatch.FilledPositions[0 % 3, 0 / 3] == thisMatch.FilledPositions[3 % 3, 3 / 3]) && (thisMatch.FilledPositions[3 % 3, 3 / 3] == thisMatch.FilledPositions[6 % 3, 6 / 3]))
        {
            win = true;
        }
        //Segunda columna
        if (thisMatch.FilledPositions[1 % 3, 1 / 3] != 3 &&  (thisMatch.FilledPositions[1 % 3, 1 / 3] == thisMatch.FilledPositions[4 % 3, 4 / 3]) && (thisMatch.FilledPositions[4 % 3, 4 / 3] == thisMatch.FilledPositions[7 % 3, 7 / 3]))
        {
            win = true;
        }
        //Tercera columna
        if (thisMatch.FilledPositions[2 % 3, 2 / 3] != 3 && (thisMatch.FilledPositions[2 % 3, 2 / 3] == thisMatch.FilledPositions[5 % 3, 5 / 3]) && (thisMatch.FilledPositions[5 % 3, 5 / 3] == thisMatch.FilledPositions[8 % 3, 8 / 3]))
        {
            win = true;
        }
        //Primera diagonal
        if (thisMatch.FilledPositions[0 % 3, 0 / 3] != 3 && (thisMatch.FilledPositions[0 % 3, 0 / 3] == thisMatch.FilledPositions[4 % 3, 4 / 3]) && (thisMatch.FilledPositions[4 % 3, 4 / 3] == thisMatch.FilledPositions[8 % 3, 8 / 3]))
        {
            win = true;
        }
        //Segunda diagonal
        if (thisMatch.FilledPositions[2 % 3, 2 / 3] != 3 && (thisMatch.FilledPositions[2 % 3, 2 / 3] == thisMatch.FilledPositions[4 % 3,4 / 3]) && (thisMatch.FilledPositions[4 % 3, 4 / 3] == thisMatch.FilledPositions[6 % 3, 6 / 3]))
        {
            win = true;
        }

        if (win)
        {  
            //Call endgame
            if (thisMatch.FilledPositions[col, row] == 0)
            {            

                if (localPlayer.Name == thisMatch.PlayerOName)
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
                if (localPlayer.Name == thisMatch.PlayerXName)
                {
                    FindObjectOfType<EndGameScript>().ShowMatchVictory();
                }
                else
                {
                    FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                }
            }
        }

        //Table full (draw)
        if (thisMatch.NumFilled == 9){
            Debug.Log("Draw");
            FindObjectOfType<EndGameScript>().ShowMatchDraw();
        }
    }

    #endregion
}
