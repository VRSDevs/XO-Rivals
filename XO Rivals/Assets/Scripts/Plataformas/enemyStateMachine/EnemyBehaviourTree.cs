using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

//Enum of types of nodes
public enum enemyNodeType{FIRST, SELECTOR, SEQUENCE, ACTION, CONDITION}
//Delegate for actions
public delegate void Action();
//Enum for operators
public enum simOperator{EQ, GT, GTE, LT, LTE}

public class enemyTree{

    //Node list
    public List<enemyNode> nodes;
    //First node
    public enemyNode firstNode;

    //Action done
    public bool actionDone;

    public enemyTree(){
        nodes = new List<enemyNode>();
        actionDone = true;
        firstNode = new enemyFirstNode();
    }

    //Only call update when done is true
    public void update(){
        if(actionDone){
            firstNode.update();
        }
    }
}

public abstract class enemyNode{

    //List of children
    public List<enemyNode> children;
    //Reference to parent
    public enemyNode parent;
    //Reference to tree
    public enemyTree treeBelongs;

    //Node type
    public enemyNodeType type;
    //Control for children update
    public int numSelect;
    public bool control, done;


    public enemyNode(enemyNode parnt){
        parent = parnt;
        children = new List<enemyNode>();
        numSelect = 0;
    }

    public void addChild(enemyNode child){
        children.Add(child);
    }

    public void addChildren(List<enemyNode> child){
        for(int i = 0; i < child.Count; i++)
            children.Add(child[i]);
    }

    public void removeChild(enemyNode child){
        children.Remove(child);
    }

    public void clearChildren(){
        children.Clear();
    }

    public abstract bool update();
    
}

public class enemyFirstNode : enemyNode{

    public enemyFirstNode() : base(null){
        type = enemyNodeType.FIRST;
    }

    //Just calls update of child
    public override bool update(){
        return children[0].update();
    }
}

public class enemySelectorNode : enemyNode{

    public enemySelectorNode(enemyNode parnt) : base(parnt){
        type = enemyNodeType.SELECTOR;
        control = false;
        done = true;
    }

    public override bool update(){
        //Selectors keep checking until children returns true
        while(!control){
            //If there are children left
            if(numSelect + 1 < children.Count){
                //Call update from children
                control = children[numSelect].update();
                //Have to update done inside actions  
            }else{
                //If no more children, go back to father
                control = true;
            }
        }
        return true;
    }
}

public class enemySequenceNode : enemyNode{

    public enemySequenceNode(enemyNode parnt) : base(parnt){
        type = enemyNodeType.SEQUENCE;
        control = true;
        done = true;
    }

    public override bool update(){
        //Sequence keep checking until child returns false
        while(control){
            //If there are children left
            if(numSelect + 1 < children.Count){
                //Call update from children
                control = children[numSelect].update();
                //Have to update done inside actions  
            }else{
                //If no more children, go back to father
                control = false;
            }
        }
        return false;
    }
}

public class enemyConditionNode<T> : enemyNode where T : System.IComparable {

    //Variable and comparing value
    T variable, compareValue;
    simOperator operat;

    //How to pass operators
    public enemyConditionNode(enemyNode parnt, simOperator op,  T param, T val) : base(parnt){
        type = enemyNodeType.CONDITION;
        variable = param;
        compareValue = val;
        operat = op;
    }

    public override bool update(){
        //Check condition
        switch(operat){

            case simOperator.EQ:
                if(variable.CompareTo(compareValue) == 0)
                    return true;
                else
                    return false;

            case simOperator.GT:
                if(variable.CompareTo(compareValue) > 0)
                    return true;
                else
                    return false;

            case simOperator.GTE:
            if(variable.CompareTo(compareValue) >= 0)
                    return true;
                else
                    return false;

            case simOperator.LT:
            if(variable.CompareTo(compareValue) < 0)
                    return true;
                else
                    return false;

            case simOperator.LTE:
            if(variable.CompareTo(compareValue) <= 0)
                    return true;
                else
                    return false;

            default:
                return false;
        }
    }

    public void updateValue(T newVal){
        variable = newVal;
    }
}

public class enemyActionNode : enemyNode{

    //Action of the actionNode
    Action nodeAction;

    public enemyActionNode(enemyNode parnt, Action nodeAct) : base(parnt){
        type = enemyNodeType.ACTION;
        nodeAction = nodeAct;
    }

    public override bool update(){
        //Do action
        nodeAction();
        return true;
    }
}

public class EnemyBehaviourTree : MonoBehaviour {

    //Character GameObject
    [SerializeField] private GameObject character;

    //Parameters
    float timeAttacking = 0f;
    private const float MAXTIMEATTACKING = 3f;
    float speed;
    private const float BASESPEED = 2.5f;
    private const float RAGESPEED = 4f;
    bool characterNear = false;
    bool isMoving = false;

    //Pathfinding route
    [SerializeField] Transform[] points;
    private Transform nextPoint;
    private int destPoint = 0;
    private float timePassed = 0f;

    //Behaviour tree
    private enemyTree enemyBT;

    void Start(){

        nextPoint = points[0];
        speed = BASESPEED;

        //Tree
        enemyBT = new enemyTree();

        //Tree nodes
        //Create condition nodes
        //enemyBT.firstNode.addChild(new enemyConditionNode<float>(enemyBT.firstNode, simOperator.EQ, 3.0f, 4.0f));

        //Create Action nodes
        //enemyBT.firstNode.addChild(new enemyActionNode(enemyBT.firstNode, new Action(Try)));
    }

    void Update(){
        //Call tree update 
        enemyBT.update();       
    }

    void Try(){
        Debug.Log("Funciona?");
    }

}