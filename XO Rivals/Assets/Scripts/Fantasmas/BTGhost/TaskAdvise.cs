using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAdvise : Node
{


    private EnemyBT _tree;


    public TaskAdvise(EnemyBT tree)
    {
        _tree = tree;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKADVISE");

        foreach(EnemyBT otherTree in _tree.otherEnemyBT)
        {
            otherTree.setAdvise(_tree.enemyNav.player);
        }
        

            state = NodeState.SUCCESS;
            return state;


    }



}
