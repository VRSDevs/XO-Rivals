using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    //Set turn to whoever starts (pass a variable from the coin flip)
    public static int turn = 0;

    //Circle and Cross
    [SerializeField] private Sprite circle;
    [SerializeField] private Sprite cross;
    private GameObject circleGO;
    private GameObject crossGO;

    //Array of positions
    private int[,] positions;

    //Variables for victory
    int col, row, filled, type;

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

        //Start variables
        filled = type = 0;
    }
    
    public void PlaceTile(int pos){

        //Get row and column
        col = pos % 3;
        row = pos / 3;
        
        //Check if position is already filled
        if(positions[col,row] == 3){
            
            //Places a sprite or another depending on turn
            if(turn == 0){
                
                GameObject newCircle = Instantiate(circleGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                newCircle.SetActive(true);

                //Change turn and save pos
                positions[col,row] = turn;
                turn = 1;
            }else{

                GameObject newCross = Instantiate(crossGO, UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.position, Quaternion.identity);
                newCross.SetActive(true);
                
                //Change turn and save pos
                positions[col,row] = turn;
                turn = 0;
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

    
