using UnityEngine;

public class ShootDroneBehaviourTree : MonoBehaviour {

    //Character GameObject
    [SerializeField] private GameObject characterO;
    [SerializeField] private GameObject characterX;
    private GameObject character;

    //GameObjects
    [SerializeField] private GameObject visionCone;
    [SerializeField] private Bullet bulletScript;

    //Parameters
    float timeBetweenShoots;
    int shootsToDouble;

    bool charDetected;

    private float timePassed = 0f;

    //Behaviour tree
    private enemyFirstNode enemyTree;
    private enemyConditionNode<bool> newConditionNode;
    private enemyConditionNode<float> newConditionNode2;

    void Start(){

        //Choose character
        character = characterO;
        bulletScript.character = character;

        //Start variables
        timeBetweenShoots = Random.Range(0.5f, 1.25f);
        shootsToDouble = (int) Random.Range(1f, 3f);
        charDetected = false;

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
        enemyActionNode newActionNode = new enemyActionNode(newSelectorNode, new Action(RotateView));
        newSelectorNode.addChild(newActionNode);

        //Level 2
        //Left Left branch -> Condition node
        newConditionNode = new enemyConditionNode<bool>(newSequenceNode, simOperator.EQ, charDetected, true);
        newSequenceNode.addChild(newConditionNode);
        //Left Right branch -> Selector node
        enemySelectorNode newSelectorNode2 = new enemySelectorNode(newSequenceNode);
        newSequenceNode.addChild(newSelectorNode2);

        //Level 3
        //Left Right Left branch -> Sequence node
        enemySequenceNode newSequenceNode2 = new enemySequenceNode(newSelectorNode2);
        newSelectorNode2.addChild(newSequenceNode2);
        //Left Right Right branch -> Action node
        enemyActionNode newActionNode2 = new enemyActionNode(newSelectorNode2, new Action(Shoot));
        newSelectorNode2.addChild(newActionNode2);

        //Level 4
        //Left Right Left Left -> Condition node
        newConditionNode2 = new enemyConditionNode<float>(newSequenceNode2, simOperator.LTE, shootsToDouble, 0);
        newSequenceNode2.addChild(newConditionNode2);
        //Left Right Left Right -> Action node
        enemyActionNode newActionNode3 = new enemyActionNode(newSequenceNode2, new Action(DoubleShoot));
        newSequenceNode2.addChild(newActionNode3);
    }

    void Update(){

        //Check character if it has been detected
        if(charDetected){
            timePassed += Time.deltaTime;
            if(timePassed >= 1f){
                timePassed = 0f;
                CheckCharacter();
            }
        }

        //Call tree update 
        enemyTree.update();       
    }

    void RotateView(){

        Debug.Log("Vigilando...");
        //Rotate cone around enemy
        if(visionCone.activeSelf)
            visionCone.transform.RotateAround(this.transform.position, Vector3.back, 20 * Time.deltaTime);
    }

    void Shoot(){

        //Check last shot
        timeBetweenShoots -= Time.deltaTime;
    
        if(timeBetweenShoots <= 0){
            Debug.Log("Pium pium");

            //Shoot a bullet
            GameObject newBullet = (GameObject) Instantiate(bulletScript.gameObject, transform.position, transform.rotation);
            newBullet.SetActive(true);

            //Restart shoot timer
            timeBetweenShoots = Random.Range(1f, 1.75f);

            //Reduce shoots to double shot
            shootsToDouble--;        
            newConditionNode2.updateValue(shootsToDouble);  
        }else
            RotateSelf(); 
    }

    void DoubleShoot(){
        
        //Check last shot
        timeBetweenShoots -= Time.deltaTime;
    
        if(timeBetweenShoots <= 0){
            Debug.Log("Doble pium pium");

            //Shoot a bullet
            GameObject newBullet1 = (GameObject) Instantiate(bulletScript.gameObject, transform.position + new Vector3(-1f, 0f, 0f), transform.rotation);
            newBullet1.SetActive(true);
            GameObject newBullet2 = (GameObject) Instantiate(bulletScript.gameObject, transform.position + new Vector3(1f, 0f, 0f), transform.rotation);
            newBullet2.SetActive(true);

            //Restart shoot timer
            timeBetweenShoots = Random.Range(0.5f, 1.25f);

            //Restart shoots to double shot
            shootsToDouble = (int) Random.Range(1f, 3f);        
            newConditionNode2.updateValue(shootsToDouble);  
        }else
            RotateSelf();
    }

    void RotateSelf(){

        Debug.Log("Croqueteo");

        //Rotate towards character    
    }

    public void CharacterDetected(bool b){

        if(b){
            Debug.Log("Te he visto");
        }else{
            Debug.Log("Pos adios");
        }

        //Update character detected
        visionCone.SetActive(!b);
        charDetected = b;
        newConditionNode.updateValue(charDetected);
    }

    void CheckCharacter(){
        if(Vector3.Distance(this.transform.position, character.transform.position) > 10)
            CharacterDetected(false);
    }

}