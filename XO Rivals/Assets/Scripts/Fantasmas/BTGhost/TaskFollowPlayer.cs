using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskFollowPlayer : Node
{

    private Transform taskTarget;
    private EnemyNavMesh _enemyNav;


    public TaskFollowPlayer(EnemyNavMesh enemyNav)
    {
        _enemyNav = enemyNav;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKFOLLOW");

        _enemyNav.target = _enemyNav.player;



        state = NodeState.RUNNING;
        return state;

    }



}
