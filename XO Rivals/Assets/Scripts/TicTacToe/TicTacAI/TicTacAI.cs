using UnityEngine;


public enum ticTacMachState {MIDEMPTY, MIDPLACED, CORNERSFULL};

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
    [SerializeField] private ButtonScriptAI butonScriptReference;
    private ScreenManagerAI screenManagerReference;

    // Start is called before the first frame update
    void Start()
    {
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

        switch(thisMatch.actualState){
            
            case ticTacMachState.MIDEMPTY:
                nextCol = 1;
                nextRow = 1;
            break;

            case ticTacMachState.MIDPLACED:
                for(int i = 0; i <= 2; i+=2){
                    for(int j = 0; j <= 2; i+=2){
                        if(thisMatch.FilledPositions[i,j] == 3){
                            nextRow = i;
                            nextCol = j;
                            goto outLoop;
                        }
                    }
                }
            outLoop : break;

            case ticTacMachState.CORNERSFULL:
                for(int i = 0; i <= 2;i++){
                    if(i%2 == 0){
                        if(thisMatch.FilledPositions[i,1] == 3){
                            nextRow = i;
                            nextCol = 1;
                            goto outLoop2;
                        }
                    }else{
                        for(int j = 0; j <= 2; j+=2){
                            if(thisMatch.FilledPositions[i,j] == 3){
                                nextRow = i;
                                nextCol = j;
                                goto outLoop2;
                            }
                        }
                    }
                }
            outLoop2: break;

        }

        PlaceTile();
    }

    void PlaceTile(){
        
        if(butonScriptReference == null){
            butonScriptReference = FindObjectOfType<ButtonScriptAI>();
            thisMatch = butonScriptReference.thisMatch;
        }
        
        //Save new position
        thisMatch.FilledPositions[nextRow,nextCol] = ai;
        thisMatch.NumFilled++;
        //thisMatch.MiniGameChosen = Random.Range(0,5);
        thisMatch.MiniGameChosen = 0;

        UpdateState();
        screenManagerReference.UpdateTurn();
    }

    public void UpdateState(){

        switch(thisMatch.actualState){

            case ticTacMachState.MIDEMPTY:
                if(thisMatch.FilledPositions[1,1] != 3)
                    thisMatch.actualState = ticTacMachState.MIDPLACED;
            break;

            case ticTacMachState.MIDPLACED:

                if(thisMatch.FilledPositions[0,0] != 3 && thisMatch.FilledPositions[0,2] != 3 && thisMatch.FilledPositions[2,0] != 3 && thisMatch.FilledPositions[2,2] != 3)
                    thisMatch.actualState = ticTacMachState.CORNERSFULL;  
            break;
        }

    }
}
