using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BTLuciernaga : BehaviorTree.Tree
{
    public EnemyNavMesh navLuciernaga;
    protected override Node SetupTree()
    {
        Node root = new TaskPatrolLuc(navLuciernaga);
        return root;
    }





}
