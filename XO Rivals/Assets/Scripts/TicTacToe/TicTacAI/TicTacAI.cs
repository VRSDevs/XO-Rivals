using UnityEngine;

public class TicTacAI : MonoBehaviour
{
    //Who is the Ai
    int player, ai;
    //Next tile location
    int nextRow, nextCol;
    //Booleans for check
    bool canWin, canLose;

    //Behaviour tree
    private enemyFirstNode ticTacTree;
    private enemyConditionNode<bool> newConditionNode, newConditionNode2;

    //Match
    private MatchAI thisMatch;
    private ButtonScriptAI butonScriptReference;
    private ScreenManagerAI screenManagerReference;

    // Start is called before the first frame update
    void Start()
    {
        butonScriptReference = FindObjectOfType<ButtonScriptAI>();
        screenManagerReference = FindObjectOfType<ScreenManagerAI>();
        thisMatch = butonScriptReference.thisMatch;

        //Must say which is player, and which is ai
        player = 0;
        ai = 1;

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
        //Left left branch  -> Conditon node
        newConditionNode = new enemyConditionNode<bool>(newSequenceNode, simOperator.EQ, canWin, true);
        newSequenceNode.addChild(newConditionNode);
        //Left right branch -> Action node
        enemyActionNode newActionNode2 = new enemyActionNode(newSequenceNode, new Action(PlaceTile));
        newSequenceNode.addChild(newActionNode2);

        //Mid left branch -> Condition node
        newConditionNode2 = new enemyConditionNode<bool>(newSequenceNode2, simOperator.EQ, canLose, true);
        newSequenceNode2.addChild(newConditionNode2);
        //Mid right branch -> Action node
        enemyActionNode newActionNode3 = new enemyActionNode(newSequenceNode2, new Action(PlaceTile));
        newSequenceNode2.addChild(newActionNode3);
    }

    public void UpdateAI(){
        canWin = canLose = false;
        canWin = CheckPosibleVictory(ai);
        canLose = CheckPosibleVictory(player);
        newConditionNode.updateValue(canWin);
        newConditionNode2.updateValue(canLose);
        ticTacTree.update();
    }

    /*private int MiniMax(int[,] thisMatch.FilledPositions, int originalTurn, int turn, int numFilled, int lastCol, int lastRow){

        //Check if its victory state
        int victory = CheckVictory(thisMatch.FilledPositions);
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
                if(thisMatch.FilledPositions[i,j] == 3){
                    //Create thisMatch.FilledPositions with new posible move
                    int[,] newthisMatch.FilledPositions = thisMatch.FilledPositions;
                    newthisMatch.FilledPositions[i,j] = turn;
                    numFilled++;

                    //Change turn & check newthisMatch.FilledPositions
                    turn = (turn == 0 ? 1 : 0);
                    int newScore = -MiniMax(newthisMatch.FilledPositions, originalTurn, turn, numFilled, i, j);

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
    bool CheckPosibleVictory(int who){

        for(int i = 0; i < 3; i++){
            if(TestCol(i, who, ref nextRow) == true)
                return true;
        }

        for(int i = 0; i < 3; i++){
            if(TestRow(i, who, ref nextCol) == true)
                return true;
        }

        for(int i = 0; i < 2; i++){
            if(TestDiag(i, who, ref nextRow, ref nextCol) == true)
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
            if(thisMatch.FilledPositions[j,col] != 3){
                //Check if its same as player
                if(thisMatch.FilledPositions[j,col] == player){
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
                if(thisMatch.FilledPositions[j,col] == 3){
                    j++;
                    if(thisMatch.FilledPositions[j,col] == player){
                        row = 1;
                        return true;
                    }else{
                        return false;
                    }
                //If mid is player, third must be empty
                }else if(thisMatch.FilledPositions[j,col] == player){
                    j++;
                    if(thisMatch.FilledPositions[j,col] == 3){
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
                if(thisMatch.FilledPositions[j,col] == player){
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
            if(thisMatch.FilledPositions[row,j] != 3){
                //Check if its same as player
                if(thisMatch.FilledPositions[row,j] == player){
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
                if(thisMatch.FilledPositions[row,j] == 3){
                    j++;
                    if(thisMatch.FilledPositions[row,j] == player){
                        col = 1;
                        return true;
                    }else{
                        return false;
                    }
                //If mid is player, third must be empty
                }else if(thisMatch.FilledPositions[row,j] == player){
                    j++;
                    if(thisMatch.FilledPositions[row,j] == 3){
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
                if(thisMatch.FilledPositions[row,j] == player){
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
                if(thisMatch.FilledPositions[j,j] != 3){
                    //Check if its same as player
                    if(thisMatch.FilledPositions[j,j] == player){
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
                    if(thisMatch.FilledPositions[j,j] == 3){
                        j++;
                        if(thisMatch.FilledPositions[j,j] == player){
                            col = row = 1;
                            return true;
                        }else{
                            return false;
                        }
                    //If mid is player, third must be empty
                    }else if(thisMatch.FilledPositions[j,j] == player){
                        j++;
                        if(thisMatch.FilledPositions[j,j] == 3){
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
                    if(thisMatch.FilledPositions[j,j] == player){
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
                if(thisMatch.FilledPositions[k,j] != 3){
                    //Check if its same as player
                    if(thisMatch.FilledPositions[k,j] == player){
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
                    if(thisMatch.FilledPositions[k,j] == 3){
                        j--;
                        k++;
                        if(thisMatch.FilledPositions[k,j] == player){
                            col = row = 1;
                            return true;
                        }else{
                            return false;
                        }
                    //If mid is player, third must be empty
                    }else if(thisMatch.FilledPositions[k,j] == player){
                        j--;
                        k++;
                        if(thisMatch.FilledPositions[k,j] == 3){
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
                    if(thisMatch.FilledPositions[k,j] == player){
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

    void StateMachine(){

        bool found = false;
        int i = 0;

        do{
            if(thisMatch.FilledPositions[i % 3,i / 3] == 3){
                nextRow = i % 3;
                nextCol = i / 3;
                found = true;
            }
            i++;
        }while(!found);

        PlaceTile();
    }

    void PlaceTile(){
        
        //Save new position
        thisMatch.FilledPositions[nextRow,nextCol] = ai;

        //Put chip
        GameObject chip = Instantiate(butonScriptReference.crossGO, butonScriptReference.botonesCuadricula[nextRow*3 + nextCol].position, Quaternion.identity);
        chip.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
        chip.SetActive(true);

        butonScriptReference.ChipsList.Add(chip);
        thisMatch.NumFilled++;
        thisMatch.MiniGameChosen = Random.Range(0,5);
        screenManagerReference.UpdateTurn();
    }
}
