using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskFollowPlayer : Node
{

    private Transform taskTarget;
    private EnemyNavMesh _enemyNav;
    private EnemyBT _tree;


    public TaskFollowPlayer(EnemyNavMesh enemyNav, EnemyBT tree)
    {
        _enemyNav = enemyNav;
        _tree = tree;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKFOLLOW");

        _tree.following = true;


        _enemyNav.target = _enemyNav.player;



        state = NodeState.RUNNING;
        return state;

    }



}
