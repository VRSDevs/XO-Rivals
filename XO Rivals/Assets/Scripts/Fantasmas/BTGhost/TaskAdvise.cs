using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAdvise : Node
{

    private EnemyNavMesh _enemyNav;
    private float time = 0;


    public TaskAdvise(EnemyNavMesh enemyNav)
    {
        _enemyNav = enemyNav;

    }




    public override NodeState Evaluate()
    {

        if (time > 3)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            time += Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }




    }



}
