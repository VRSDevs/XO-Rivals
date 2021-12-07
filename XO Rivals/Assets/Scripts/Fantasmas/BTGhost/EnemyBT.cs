using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyBT : BehaviorTree.Tree
{
    public EnemyNavMesh enemyNav;


    protected override Node SetupTree()
    {


        Node root = new TaskPatrol(enemyNav);
        return root;

    }
}
