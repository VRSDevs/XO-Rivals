using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckRadio : Node
{
    private DetectPlayer _detectPlayer;

    public CheckRadio(DetectPlayer detectPlayer)
    {
        _detectPlayer = detectPlayer;

    }



    public override NodeState Evaluate()
    {

        if (_detectPlayer.veoPlayerRadio) //DETECTA LA LINTERNA AL JUGADOR
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
