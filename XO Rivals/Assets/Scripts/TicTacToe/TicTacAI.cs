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

    /*private int MiniMax(int[,] board, int originalTurn, int turn, int numFilled, int lastCol, int lastRow){

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
    }*/
    
    #region TestMethods

    //Optimize this using last tile
    bool CheckVictory(int[,] board, int player, ref int row, ref int col){

        for(int i = 0; i < 3; i++){
            if(TestCol(i, player, ref row) == true)
                return true;
        }

        for(int i = 0; i < 3; i++){
            if(TestRow(i, player, ref col) == true)
                return true;
        }

        for(int i = 0; i < 2; i++){
            if(TestDiag(i, player, ref row, ref col) == true)
               return true;
        }

        return false;
    }

    bool TestCol(int col, int player, ref int row){

        bool found = false;
        int j = -1;

        //Pick first tile in column that is not empty
        do{
            //If its the last tile, its imposible to win
            j++;
            if(j == 2){
                return false;
            }

            //If its not empty
            if(board[j,col] != 3){
                //Check if its same as player
                if(board[j,col] == player){
                    found = true;
                }else{
                    return false;
                }
            }
            
        }while(!found);

        //Check rest of the tiles
        j++;
        switch(j){

            //If found on first, there are two options 
            case 1:
                //If mid is empty, third must be player
                if(board[j,col] == 3){
                    j++;
                    if(board[j,col] == player){
                        row = 1;
                        return true;
                    }else{
                        return false;
                    }
                //If mid is player, third must be empty
                }else if(board[j,col] == player){
                    j++;
                    if(board[j,col] == 3){
                        row = j;
                        return true;
                    }else{
                        return false;
                    }
                }else{
                    return false;
                }

            //If found on mid, last must match
            case 2:
                if(board[j,col] == player){
                    row = 0;
                    return true;
                }else{
                    return false;
                }
        }

        return false;
    }
    
    bool TestRow(int row, int player, ref int col){

        bool found = false;
        int j = -1;

        //Pick first tile in column that is not empty
        do{
            //If its the last tile, its imposible to win
            j++;
            if(j == 2){
                return false;
            }

            //If its not empty
            if(board[row,j] != 3){
                //Check if its same as player
                if(board[row,j] == player){
                    found = true;
                }else{
                    return false;
                }
            }
            
        }while(!found);

        //Check rest of the tiles
        j++;
        switch(j){

            //If found on first, there are two options 
            case 1:
                //If mid is empty, third must be player
                if(board[row,j] == 3){
                    j++;
                    if(board[row,j] == player){
                        col = 1;
                        return true;
                    }else{
                        return false;
                    }
                //If mid is player, third must be empty
                }else if(board[row,j] == player){
                    j++;
                    if(board[row,j] == 3){
                        col = j;
                        return true;
                    }else{
                        return false;
                    }
                }else{
                    return false;
                }

            //If found on mid, last must match
            case 2:
                if(board[row,j] == player){
                    col = 0;
                    return true;
                }else{
                    return false;
                }
        }

        return false;
    }

    bool TestDiag(int diag, int player, ref int row, ref int col){

        bool found = false;
        int j = -1;

        //First diagonal
        if(diag == 0){

            //Pick first tile in diagonal that is not empty
            do{
                //If its the last tile, its imposible to win
                j++;
                if(j == 2){
                    return false;
                }

                //If its not empty
                if(board[j,j] != 3){
                    //Check if its same as player
                    if(board[j,j] == player){
                        found = true;
                    }else{
                        return false;
                    }
                }
            }while(!found);

            //Check rest of the tiles
            j++;
            switch(j){

                //If found on first, there are two options 
                case 1:
                    //If mid is empty, third must be player
                    if(board[j,j] == 3){
                        j++;
                        if(board[j,j] == player){
                            col = row = 1;
                            return true;
                        }else{
                            return false;
                        }
                    //If mid is player, third must be empty
                    }else if(board[j,j] == player){
                        j++;
                        if(board[j,j] == 3){
                            col = row = j;
                            return true;
                        }else{
                            return false;
                        }
                    }else{
                        return false;
                    }

                //If found on mid, last must match
                case 2:
                    if(board[j,j] == player){
                        col = row = 0;
                        return true;
                    }else{
                        return false;
                    }
            }

            return false;

        //Second diagonal
        }else{

            j = 3;
            int k = -1;

            //Pick first tile in column that is not empty
            do{
                //If its the last tile, its imposible to win
                j--;
                k++;
                if(k == 2){
                    return false;
                }

                //If its not empty
                if(board[k,j] != 3){
                    //Check if its same as player
                    if(board[k,j] == player){
                        found = true;
                    }else{
                        return false;
                    }
                }
                
            }while(!found);

            //Check rest of the tiles
            j--;
            k++;
            switch(k){

                //If found on first, there are two options 
                case 1:
                    //If mid is empty, third must be player
                    if(board[k,j] == 3){
                        j--;
                        k++;
                        if(board[k,j] == player){
                            col = row = 1;
                            return true;
                        }else{
                            return false;
                        }
                    //If mid is player, third must be empty
                    }else if(board[k,j] == player){
                        j--;
                        k++;
                        if(board[k,j] == 3){
                            row = k;
                            col = j;
                            return true;
                        }else{
                            return false;
                        }
                    }else{
                        return false;
                    }

                //If found on mid, last must match
                case 2:
                    if(board[k,j] == player){
                        row = j;
                        col = k;
                        return true;
                    }else{
                        return false;
                    }
            }

            return false;
        }
    }

    #endregion
}
