using UnityEngine;

public class TicTacAI : MonoBehaviour
{
    int[,] board;

    //Behaviour tree
    private enemyFirstNode ticTacTree;
    private enemyConditionNode<bool> newConditionNode, newConditionNode2;

    bool canWin, canLose;
    int winner, chosenCol, chosenRow;

    // Start is called before the first frame update
    void Start()
    {
        board = new int[3,3];

        //Tree
        ticTacTree = new enemyFirstNode();

        //Tree nodes
        //Level 0
        //Main node -> Selector node
        enemySelectorNode newSelectorNode = new enemySelectorNode(ticTacTree);
        ticTacTree.addChild(newSelectorNode);

        //Level 1
        //Left branch -> Sequence node
        enemySequenceNode newSequenceNode = new enemySequenceNode(newSelectorNode);
        newSelectorNode.addChild(newSequenceNode);
        //Mid branch -> Sequence node
        enemySequenceNode newSequenceNode2 = new enemySequenceNode(newSelectorNode);
        newSelectorNode.addChild(newSequenceNode2);
        //Right branch -> Action node
        enemyActionNode newActionNode = new enemyActionNode(newSelectorNode, new Action(StateMachine));
        newSelectorNode.addChild(newActionNode);

        //Level 2
        //Left Left branch -> Condition node
        newConditionNode = new enemyConditionNode<bool>(newSequenceNode, simOperator.EQ, canWin, true);
        newSequenceNode.addChild(newConditionNode);
        //Left Right branch -> Action node
        enemyActionNode newActionNode2 = new enemyActionNode(newSequenceNode, new Action(PlaceWin));

        //Mid Left branch -> Condition node
        newConditionNode2 = new enemyConditionNode<bool>(newSequenceNode2, simOperator.EQ, canLose, true);
        //Mid Right branch -> Action node
        enemyActionNode newActionNode3 = new enemyActionNode(newSequenceNode2, new Action(PlaceLose));
    }

    // Update is called once per frame
    void UpdateTurn()
    {
        winner = CheckVictory(board, ref chosenCol, ref chosenRow);
        canWin = canLose = false;

        switch(winner){
            //X wins
            case -1:
                canWin = true;
            break;

            //O wins
            case 1:
                canLose = true;
            break;
        }
        newConditionNode.updateValue(canWin);
        newConditionNode2.updateValue(canLose);

        ticTacTree.update();
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
    int CheckVictory(int[,] board, ref int col, ref int row){

        for(int i = 0; i < 3; i++){
            if(TestCol(i, ref row) == true){
                col = i;
                return board[row, col] == 0 ? -1 : 1;
            }
        }

        for(int i = 0; i < 3; i++){
            if(TestRow(i, ref col) == true){
                row = i;
                return board[row, col] == 0 ? -1 : 1;
            }
        }

        for(int i = 0; i < 2; i++){
            if(TestDiag(i, ref col, ref row) == true)
               return board[1,1] == 0 ? -1 : 1;
        }

        return 0;
    }

    bool TestCol(int col, ref int row){
        int type = -1;
        int j = -1;

        //Pick first tile in column that its not empty
        do{     

            j++;
            //If first two are empty, there is no need to continue
            if(j == 2)
                return false;

            if(board[j,col] != 3){
                type = board[j,col];
            }
        }while(type == -1);

        //Check other positions in col
        j++;
        switch(j){
            
            //We have two chances to have victory
            case 1:
                //If mid is empty, check third
                if(board[j,col] == 3){
                    //Cant make that line if other is not mine (empty or enemy)
                    j++;
                    if(board[j,col] != type){
                        return false;
                    }else{
                        //If its mine, mid place is win
                        row = 1;
                        return true;
                    }
                //If mine, check if third is empty
                }else if(board[j,col] == type){
                    j++;
                    if(board[j,col] != 3)
                        return false;
                    else{
                        row = j;
                        return true;
                    }
                //If mid is of opponent, cant win
                }else{
                    return false;
                }

            //If first one was empty, this must match prev
            case 2:
                if(board[col,j] == type){
                    row = 0;
                    return true;
                }else{
                    return false;
                }
        }

        return false;
    }
    
    bool TestRow(int row, ref int col){
        int type = -1;
        int j = -1;

        //Pick first tile in column that its not empty
        do{     

            j++;
            //If first two are empty, there is no need to continue
            if(j == 2)
                return false;

            if(board[row,j] != 3){
                type = board[row,j];
            }
        }while(type == -1);

        //Check other positions in col
        j++;
        switch(j){
            
            //We have two chances to have victory
            case 1:
                //If mid is empty, check third
                if(board[row,j] == 3){
                    //Cant make that line if other is not mine (empty or enemy)
                    j++;
                    if(board[row,j] != type){
                        return false;
                    }else{
                        //If its mine, mid place is win
                        col = 1;
                        return true;
                    }
                //If mine, check if third is empty
                }else if(board[row,j] == type){
                    j++;
                    if(board[row,j] != 3)
                        return false;
                    else{
                        col = j;
                        return true;
                    }
                //If mid is of opponent, cant win
                }else{
                    return false;
                }

            //If first one was empty, this must match prev
            case 2:
                if(board[row,j] == type){
                    col = 0;
                    return true;
                }else{
                    return false;
                }
        }

        return false;
    }

    bool TestDiag(int diag, ref int col, ref int row){
        int type = -1;
        int j = -1;

        //First diagonal
        if(diag == 0){

            //Pick first tile in diagonal that its not empty
            do{            
                j++;
                //If first two are empty, there is no need to continue
                if(j == 2)
                    return false;

                if(board[j,j] != 3){
                    type = board[j,j];
                }

            }while(type == -1);

            //Check other positions in diag
            j++;
            switch(j){
                
                //We have two chances to have victory
                case 1:
                    //If mid is empty
                    if(board[j,j] == 3){
                        //You got to check the other one
                        j++;
                        if(board[j, j] != type){
                            return false;
                        }else{
                            row = col = j;
                            return true;
                        }
                    //If mid is mine
                    }else if(board[j,j] == type){
                        j++;
                        //If last is not empty, cant win
                        if(board[j,j] != 3){
                            return false;
                        }else{
                            row = col = j;
                            return true;
                        }
                    //If mid is opponent, cant win
                    }else{
                        return false;
                    }

                //If first one was empty, this must match prev
                case 2:
                    if(board[j,j] == type){
                        row = col = 0;
                        return true;
                    }else{
                        return false;
                    }
            }
        }else{
            
            j = 3;
            int k = -1;
            //Pick first tile in diagonal that its not empty
            do{            
                j--;
                k++;
                //If first two are empty, there is no need to continue
                if(k == 2)
                    return false;

                if(board[k,j] != 3){
                    type = board[j,j];
                }

            }while(type == -1);

            //Check other positions in diag
            j--;
            k++;
            switch(k){
                
                //We have two chances to have victory
                case 1:
                    //If mid is empty
                    if(board[k,j] == 3){
                        //You got to check the other one
                        k++;
                        j--;
                        if(board[k, j] != type){
                            return false;
                        }else{
                            row = col = 1;
                            return true;
                        }
                    //If mid is mine
                    }else if(board[k,j] == type){
                        k++;
                        j--;
                        //If last is not empty, cant win
                        if(board[k,j] != 3){
                            return false;
                        }else{
                            row = k;
                            col = j;
                            return true;
                        }
                    //If mid is opponent, cant win
                    }else{
                        return false;
                    }

                //If first one was empty, this must match prev
                case 2:
                    if(board[k,j] == type){
                        row = 0;
                        col = 2;
                        return true;
                    }else{
                        return false;
                    }
            }
        }

        return false;
    }

    #endregion

    void StateMachine(){

    }

    void PlaceWin(){

    }

    void PlaceLose(){

    }
}
