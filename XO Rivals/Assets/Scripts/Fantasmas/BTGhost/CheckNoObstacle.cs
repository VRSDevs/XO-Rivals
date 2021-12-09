using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckNoObstacle : Node
{

    private DetectPlayer _detectPlayer;

    public CheckNoObstacle(DetectPlayer detectPlayer)
    {
        _detectPlayer = detectPlayer;

    }



    public override NodeState Evaluate()
    {

        if (_detectPlayer.veoPlayerDistance) //DETECTA LA LINTERNA AL JUGADOR
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else //NO DETECTA
        {
            state = NodeState.FAILURE;
            return state;
        }


    }




}
