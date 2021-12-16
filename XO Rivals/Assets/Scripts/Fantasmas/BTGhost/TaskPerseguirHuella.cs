using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPerseguirHuella : Node
{


    private EnemyBT _tree;
    private EnemyNavMesh _enemyNav;


    public TaskPerseguirHuella(EnemyBT tree, EnemyNavMesh enemyNav)
    {
        _tree = tree;
        _enemyNav = enemyNav;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKPERSEGUIRHUELLA");

        foreach (Transform t in _tree.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Enemy2D")
            {
                //Cosas que hacer
                Debug.Log(t.name);
                t.GetComponent<Animator>().SetBool("Ask", true);
                t.GetComponent<Animator>().SetBool("Normal", false);
                t.GetComponent<Animator>().SetBool("Alert", false);

            }
        }



        _tree.following = false;

        if (_tree.lastHuella == null)//SI ha desaparecido la huella deja de perseguirlas
        {
            _tree.perseguirHuella = false;
        }
        else
        {
            ScriptHuella huella = _tree.lastHuella.GetComponent<ScriptHuella>();

            _enemyNav.target.position = huella.sigHuella.transform.position;
        }
       
      

        state = NodeState.RUNNING;
        return state;

    }
}
