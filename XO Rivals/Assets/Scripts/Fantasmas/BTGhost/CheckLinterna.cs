using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckLinterna : Node
{
    private DetectPlayer _detectPlayer;

    public CheckLinterna(DetectPlayer detectPlayer)
    {
        _detectPlayer = detectPlayer;

    }



    public override NodeState Evaluate()
    {

        if (_detectPlayer.linernaDetect) //DETECTA LA LINTERNA AL JUGADOR
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
