using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

public class ButtonsScript : MonoBehaviourPun
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


    public string whosName;
    public string auxPLayerX;
    public string auxPLayerO;
    public int auxMinijuego;


    private void Awake()
    {
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
        if (gameState.PlayerInfoX == null)
        {

            //Fill array
            gameState.filledPositions = new int[3, 3];
            for (int i = 0; i < gameState.filledPositions.GetLength(0); i++)
            {
                for (int j = 0; j < gameState.filledPositions.GetLength(1); j++)
                {
                    gameState.filledPositions[i, j] = 3;
                }
            }

            //Start variables
            gameState.numFilled = 0;
            gameState.chips = new List<GameObject>();

            //El minijuego es elegido automaticamente
            gameState.miniGameChosen = Random.Range(0, 2);
            gameState.turnMoment = 0;
        }

        //Initialize ScreenManager
        screenManager = FindObjectOfType<ScreenManager>();
    }
  

    public void Start()
    {
        //Fill array
        gameState.filledPositions = new int[3, 3];
        for (int i = 0; i < gameState.filledPositions.GetLength(0); i++)
        {
            for (int j = 0; j < gameState.filledPositions.GetLength(1); j++)
            {
                gameState.filledPositions[i, j] = 3;
            }
        }
        gameState.numFilled = 0;

        PhotonView photonView = PhotonView.Get(this);

        if (gameState.PlayerInfoX != null)
        {

            photonView.RPC("actualizarplayerX", RpcTarget.All, gameState.PlayerInfoX.Name);


        }
        if (gameState.PlayerInfoO != null)
        {

            photonView.RPC("actualizarplayerO", RpcTarget.All, gameState.PlayerInfoO.Name);


        }


        if (gameState.WhosTurn != null)
        {

            Debug.Log(gameState.WhosTurn.Name + "NOMBREE");
            photonView.RPC("actualizarWhosName", RpcTarget.All, gameState.WhosTurn.Name);

            photonView.RPC("cargarTurno", RpcTarget.All);

            //ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
            //setValue.Add("whosturn", gameState.WhosTurn);
            //PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);


        }

        





    }

    public void PlaceTile(int pos)
    {

        //Get row and column
        col = pos % 3;
        row = pos / 3;

        //Check if position is already filled
        if (gameState.filledPositions[col, row] == 3)
        {
            
            //Places a sprite or another depending on turn
            if (auxPLayerO == localPlayer.Name)
            {

                //Place chip
                actualChip = Instantiate(circleGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                actualChip.SetActive(true);
                actualChip.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.35f);
                gameState.turnMoment = 1;

                //Add chip to list to hide
                gameState.chips.Add(actualChip);
                for (int i = 0; i < gameState.chips.Count; i++)
                    gameState.chips[i].SetActive(false);

                //Go to minigame
                PlayMinigame();

               

                //Go to selectMinigame for opponent
                //screenManager.MinigameSelectionActivation();

                //Save pos
                //gameState.filledPositions[col, row] = 0;

                //Disable input because its not your turn
                //screenManager.DisableButtons();

            }
            else
            {

                //Place chip
                actualChip = Instantiate(crossGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                actualChip.SetActive(true);
                actualChip.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.35f);
                gameState.turnMoment = 1;

                //Add chip to list to hide
                gameState.chips.Add(actualChip);
                for (int i = 0; i < gameState.chips.Count; i++)
                    gameState.chips[i].SetActive(false);

                //Go to minigame
                PlayMinigame();

               

                //Go to selectMinigame for opponent
               // screenManager.MinigameSelectionActivation();

                //Save pos
                //gameState.filledPositions[col, row] = 1;

                //Disable input because its not your turn
               // screenManager.DisableButtons();
            }



            //Add one to count
            //gameState.numFilled++;

            //Check victory
            CheckVictory();

            //Table full (draw)
            if (gameState.numFilled == 9)
            {
                Debug.Log("Draw");
            }
        }
        else
        {
            Debug.Log("Tile not empty");
        }
    }

    private void PlayMinigame()
    {

        miniWin = false;
        switch (gameState.miniGameChosen)
        {
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
        Debug.Log(miniWin);
        if (miniWin == true)
        {

            //Save position
             photonView.RPC("actualizarTablero", RpcTarget.All);
            //Paint tile completely
            actualChip.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);




            //ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
            //setValue.Add("col_"+col+"row_"+row, (gameState.WhosTurn == gameState.PlayerInfoO ? 0 : 1));
            //PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);

            Debug.Log("ACTUALIZA PROPERTIS");

        }
        else
        {
            Destroy(actualChip);
            gameState.chips.Remove(actualChip);
            StartCoroutine(screenManager.txtTimer("¡Turno perdido!"));




        }
        gameState.turnMoment = 2;
        //Debug.Log("TE CAMBIO TURNO a " + 2);
        screenManager.MinigameSelectionActivation();

        Debug.Log("HA ACABADO EL JUEGO PARA MI");
        //photonView.RPC("cargarTurno", RpcTarget.All);


    }

    public void CheckVictory()
    {

        bool[] array = new bool[8];

        //Fill array with true every loop
        for (int w = 0; w < 8; w++)
        {
            array[w] = true;
        }

        //If they are not equal, they are not on the main diagonal
        if (col != row)
        {
            array[6] = false;
        }

        //If they dont add 2, they are not on the secondary diagonal
        if (col + row != 2)
        {
            array[7] = false;
        }

        //Check column
        switch (col)
        {
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
        switch (row)
        {
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
        do
        {
            if (i < 3)
            {
                win = TestCol(i);
            }
            else if (i < 6)
            {
                win = TestRow(i % 3);
            }
            else
            {
                win = TestDiag(i % 6);
            }

            if (win)
            {
                PlayerInfo localPlayer = FindObjectOfType<PlayerInfo>();

                gameState.IsPlaying = false;

                //Call endgame
                if (gameState.filledPositions[col, row] == 0)
                {
                    Debug.Log("CIRCLE WIN");

                    if (localPlayer.Name == auxPLayerO)
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

                    if (localPlayer.Name == auxPLayerX)
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
        } while (i < 8);
    }

    bool TestCol(int col)
    {
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if (gameState.filledPositions[col, j] != 3)
        {
            type = gameState.filledPositions[col, j];
            j++;
        }
        else
        {
            return false;
        }

        //Check if all other tiles are the same
        do
        {
            if (gameState.filledPositions[col, j] != type)
            {
                return false;
            }
            j++;
        } while (j < 3);

        return true;
    }

    bool TestRow(int row)
    {
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if (gameState.filledPositions[j, row] != 3)
        {
            type = gameState.filledPositions[j, row];
            j++;
        }
        else
        {
            return false;
        }

        //Check if all other tiles are the same
        do
        {
            if (gameState.filledPositions[j, row] != type)
            {
                return false;
            }
            j++;
        } while (j < 3);

        return true;
    }

    bool TestDiag(int diag)
    {
        int type;
        int j = 0;

        //First diagonal
        if (diag == 0)
        {
            //Pick first tile in column if its not empty
            if (gameState.filledPositions[diag, j] != 3)
            {
                type = gameState.filledPositions[diag, j];
                j++;
                diag++;
            }
            else
            {
                return false;
            }

            //Check if all other tiles are the same
            do
            {
                if (gameState.filledPositions[diag, j] != type)
                {
                    return false;
                }
                diag++;
                j++;
            } while (j < 3);

            return true;

            //Second diagonal
        }
        else
        {
            diag++;
            //Pick first tile in column if its not empty
            if (gameState.filledPositions[diag, j] != 3)
            {
                type = gameState.filledPositions[diag, j];
                j++;
                diag--;
            }
            else
            {
                return false;
            }

            //Check if all other tiles are the same
            do
            {
                if (gameState.filledPositions[diag, j] != type)
                {
                    return false;
                }
                diag--;
                j++;
            } while (j < 3);

            return true;
        }
    }


    [PunRPC]
    public void cargarTurno()
    {
        Debug.Log("CARGO TURNO");
       Debug.Log(whosName + " WHOO");
 
        Debug.Log(localPlayer.Name);
       



        //If its your turn, play, if its not, only can see
        if (whosName == localPlayer.Name)
        {
  
            Debug.Log("ES MI TURNO");

            //Depending of turn moment, player will encounter a "different scene"
            //if(turnInstant == 0){
            //Nothing happens
            /*}else*/
            if (gameState.turnMoment == 1)
            {
                //Go directly to minigame
                
                //PlayMinigame();
            }
            else if (gameState.turnMoment == 2)
            {
                //Go to choose minigame
         
                //screenManager.MinigameSelectionActivation();
            }
        }
        else
        {
         
            Debug.Log("NO ES MI TURNO");
            //Disable interaction with tictac cause its not your turn
            screenManager.DisableButtons();

        }
        


    }

   
    public void actualizarTurno(int minijuego)
    {

        photonView.RPC("actualizarTurnoRPC", RpcTarget.All,minijuego);


    }



    [PunRPC]
    public void actualizarTurnoRPC(int minijuego)
    {
        auxMinijuego = minijuego;


        //Unihide chips
        for (int i = 0; i < gameState.chips.Count; i++)
        {
            if (gameState.chips[i] == null)
            {
                continue;
            }
            gameState.chips[i].SetActive(true);
        }



        Debug.Log("UPDATEAMOS TURNO");

        if (auxPLayerO == whosName)
        {
            whosName = auxPLayerX;
        }

        else
        {
            whosName =auxPLayerO;
        }


        Debug.Log("TE CAMBIO TURNO a " + 0);
        gameState.turnMoment = 0;
        //Display turn in screen
        if (whosName == auxPLayerO)
        {
            StartCoroutine(screenManager.txtTimer("Turno de O"));
        }
        else
        {
            StartCoroutine(screenManager.txtTimer("Turno de X"));
        }


        if (localPlayer.Name == whosName)
        {


            screenManager.EnableButtons();
            Debug.Log("ACTIVAMOS BOTONES PARA MI");



        }

    }



    [PunRPC]
    public void actualizarWhosName(string player)
    {

        whosName = player;


    }

    [PunRPC]
    public void actualizarplayerX(string player)
    {

        auxPLayerX = player;


    }

    [PunRPC]
    public void actualizarplayerO(string player)
    {

        auxPLayerO = player;


    }


    [PunRPC]
    public void actualizarTablero()
    {

        gameState.filledPositions[col, row] = (whosName == auxPLayerO ? 0 : 1);
        gameState.numFilled++;

       

    }





}
