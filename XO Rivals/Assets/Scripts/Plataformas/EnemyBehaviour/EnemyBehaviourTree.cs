using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

//Enum of types of nodes
public enum enemyNodeType{FIRST, SELECTOR, SEQUENCE, ACTION, CONDITION}
//Delegate for actions
public delegate void Action();
//Enum for operators
public enum simOperator{EQ, GT, GTE, LT, LTE}

public abstract class enemyNode{

    //List of children
    public List<enemyNode> children;
    //Reference to parent
    public enemyNode parent;

    //Node type
    public enemyNodeType type;
    //Control for children update
    public int numSelect;
    public bool control;


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
    }

    public override bool update(){
        //Reset value every update
        control = false;
        numSelect = 0;

        //If there are children left
        while(numSelect < children.Count){
            //Call update from children
            control = children[numSelect].update();
            numSelect++;

            if(control)
                return control;
        }
        return control;
    }
}

public class enemySequenceNode : enemyNode{

    public enemySequenceNode(enemyNode parnt) : base(parnt){
        type = enemyNodeType.SEQUENCE;
        control = true;
    }

    public override bool update(){
        //Reset value every update
        control = true;
        numSelect = 0;

        //If there are children left
        while(numSelect < children.Count){
            //Call update from children
            control = children[numSelect].update();
            numSelect++;

            if(!control)
                return control;
        }
        return control;
    }
}

public class enemyConditionNode<T> : enemyNode where T : System.IComparable {

    //Variable and comparing value
    T variable, compareValue;
    simOperator operat;

    public enemyConditionNode(enemyNode parnt, simOperator op,  T param, T val) : base(parnt){
        type = enemyNodeType.CONDITION;
        operat = op;
        variable = param;
        compareValue = val;
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

    float distanceToChar;
    private const float MAXDISTANCE = 5.0f;

    //Pathfinding route
    [SerializeField] Transform[] points;
    private Transform nextPoint;
    private int destPoint = 0;
    private float timePassed = 0f;

    //Behaviour tree
    private enemyFirstNode enemyTree;
    private enemyConditionNode<float> newConditionNode, newConditionNode2;

    void Start(){

        //Start variables
        nextPoint = points[0];
        speed = BASESPEED;
        distanceToChar = CheckCharacter();

        //Tree
        enemyTree = new enemyFirstNode();

        //Tree nodes
        //Level 0
        //Main node -> Selector node
        enemySelectorNode newSelectorNode = new enemySelectorNode(enemyTree);
        enemyTree.addChild(newSelectorNode);

        //Level 1
        //Left branch -> Sequence node
        enemySequenceNode newSequenceNode = new enemySequenceNode(newSelectorNode);
        newSelectorNode.addChild(newSequenceNode);
        //Right branch -> Action node
        enemyActionNode newActionNode = new enemyActionNode(newSelectorNode, new Action(GotoNextPoint));
        newSelectorNode.addChild(newActionNode);

        //Level 2
        //Left Left branch -> Condition node
        newConditionNode = new enemyConditionNode<float>(newSequenceNode, simOperator.LTE, distanceToChar, MAXDISTANCE);
        newSequenceNode.addChild(newConditionNode);
        //Left Right branch -> Selector node
        enemySelectorNode newSelectorNode2 = new enemySelectorNode(newSequenceNode);
        newSequenceNode.addChild(newSelectorNode2);

        //Level 3
        //Left Right Left branch -> Sequence node
        enemySequenceNode newSequenceNode2 = new enemySequenceNode(newSelectorNode2);
        newSelectorNode2.addChild(newSequenceNode2);
        //Left Right Right branch -> Action node
        enemyActionNode newActionNode2 = new enemyActionNode(newSelectorNode2, new Action(GoToCharacter));
        newSelectorNode2.addChild(newActionNode2);

        //Level 4
        //Left Right Left Left -> Condition node
        newConditionNode2 = new enemyConditionNode<float>(newSequenceNode2, simOperator.GTE, timeAttacking, MAXTIMEATTACKING);
        newSequenceNode2.addChild(newConditionNode2);
        //Left Right Left Right -> Action node
        enemyActionNode newActionNode3 = new enemyActionNode(newSequenceNode2, new Action(GoToCharacterRage));
        newSequenceNode2.addChild(newActionNode3);
    }

    void Update(){

        //Check character every second
        timePassed += Time.deltaTime;
        if(timePassed >= 1.0f){
            distanceToChar = CheckCharacter();
            timePassed = 0f;
            //Update value of condition node
            newConditionNode.updateValue(distanceToChar);
        }
        newConditionNode2.updateValue(timeAttacking);    
            
        //Call tree update 
        enemyTree.update();       
    }

    void GotoNextPoint(){

        //Reset time attacking
        timeAttacking = 0f;
        speed = BASESPEED;

        if(Vector3.Distance(this.transform.position, nextPoint.transform.position) < 0.5f){
            //Set next point as destination
            nextPoint = points[destPoint];
        
            //Go to next point or cycle to first one
            destPoint = (destPoint + 1) % points.Length;
        }

        //Go to actual point
        this.transform.position = Vector3.MoveTowards(this.transform.position, nextPoint.transform.position, Time.deltaTime * speed);
    }
    
    void GoToCharacter(){

        //Set velocity
        speed = BASESPEED;
        timeAttacking += Time.deltaTime;

        //Go to actual point
        this.transform.position = Vector3.MoveTowards(this.transform.position, character.transform.position, Time.deltaTime * speed);
    }

    void GoToCharacterRage(){

        //Set velocity
        speed = RAGESPEED;

        //Go to actual point
        this.transform.position = Vector3.MoveTowards(this.transform.position, character.transform.position, Time.deltaTime * speed);
    }

    float CheckCharacter(){
        return Vector3.Distance(this.transform.position, character.transform.position);
    }

}