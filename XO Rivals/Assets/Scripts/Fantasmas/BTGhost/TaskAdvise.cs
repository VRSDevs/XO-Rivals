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



        _tree.lastSeenPlayer = new Vector3(_tree.enemyNav.player.position.x, _tree.enemyNav.player.position.y, _tree.enemyNav.player.position.z);
        foreach (EnemyBT otherTree in _tree.otherEnemyBT)
        {
            otherTree.setAdvise(_tree.lastSeenPlayer);
        }
        

            state = NodeState.SUCCESS;
            return state;


    }



}
