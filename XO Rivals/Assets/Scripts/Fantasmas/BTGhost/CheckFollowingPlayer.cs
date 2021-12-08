using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckFollowingPlayer : Node
{

    private EnemyBT _tree;



    public CheckFollowingPlayer(EnemyBT tree)
    {
        _tree = tree;

    }




    public override NodeState Evaluate()
    {


        if (_tree.following) //SI ESTA DADO AVISO NO PARA HASTA LLEGAR AL AVISO
        {

            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }


    }
}
