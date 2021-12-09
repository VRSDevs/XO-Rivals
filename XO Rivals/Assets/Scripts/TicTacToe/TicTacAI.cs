using UnityEngine;

public class TicTacAI : MonoBehaviour
{

    int[,] board;

    // Start is called before the first frame update
    void Start()
    {
        board = new int[3,3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int MiniMax(int[,] board, int originalTurn, int turn, int numFilled, int lastCol, int lastRow){

        //Check if its victory state
        int victory = CheckVictory(board);
        if(victory != 0){
            //Return 1 if its player win, -1 if its opponent
            return victory == originalTurn ? 1 : -1;
        }

        //If its not terminal, create score for this node
        int score = -2;
        int move = -1;

        //Create child for every move
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                //If place is empty
                if(board[i,j] == 3){
                    //Create board with new posible move
                    int[,] newBoard = board;
                    newBoard[i,j] = turn;
                    numFilled++;

                    //Change turn & check newBoard
                    turn = (turn == 0 ? 1 : 0);
                    int newScore = -MiniMax(newBoard, originalTurn, turn, numFilled, i, j);

                    //Save new score if its better for the opponent??
                    if(newScore > score){
                        score = newScore;
                        move = i;
                    }
                }
            }
        }

        //If no more moves, return 0
        if(move == -1){
            return 0;
        }

        return score;
    }

    
    #region TestMethods

    //Optimize this using last tile
    int CheckVictory(int[,] board){

        for(int i = 0; i < 3; i++){
            if(TestCol(i) == true)
                return board[i,0] == 0 ? -1 : 1;
        }

        for(int i = 0; i < 3; i++){
            if(TestRow(i) == true)
                return board[i,0] == 0 ? -1 : 1;
        }

        for(int i = 0; i < 2; i++){
            if(TestDiag(i) == true)
               return board[i,0] == 0 ? -1 : 1;
        }

        return 0;
    }

    bool TestCol(int col){
        int type;
        int j = 0;

        //Pick first tile in column if its not empty
        if(board[col,j] != 3){
            type = board[col,j];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(board[col,j] != type){
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
        if(board[j,row] != 3){
            type = board[j,row];
            j++;
        }else{
            return false;
        }

        //Check if all other tiles are the same
        do{
            if(board[j,row] != type){
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
            if(board[diag,j] != 3){
                type = board[diag,j];
                j++;
                diag++;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(board[diag,j] != type){
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
            if(board[diag,j] != 3){
                type = board[diag,j];
                j++;
                diag--;
            }else{
                return false;
            }

            //Check if all other tiles are the same
            do{
                if(board[diag,j] != type){
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
