using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

//Enum of different states
public enum robotState{WALKING, ATTACKING, JUMPING, REROUTE}

public class RobotStateMachine : MonoBehaviour{
    
    //Character GameObject
    [SerializeField] private GameObject characterO;
    [SerializeField] private GameObject characterX;
    private GameObject characterPlaying;
    
    //Game variables
    private GameManager gameState;
    private Match thisMatch;
    private PlayerInfo localPlayer;    

    //State
    robotState actualState = robotState.WALKING;

    //Parameters
    private float timeAttacking = 0f;
    private const float MAXTIMEATTACKING = 2f;
    private float timeToJump;
    private const float speed = 2.5f;
    private const float jumpSpeed = 5f;
    private bool characterNear = false;
    private bool jumping = false;

    //Pathfinding route
    [SerializeField] Transform[] points;
    private Transform nextPoint;
    private int destPoint = 0;
    private float timePassed = 0f;
    private float originalY, maxY;
    private bool jumpUp = true;

    void Start(){

        nextPoint = points[0].transform;

        gameState = FindObjectOfType<GameManager>();
        thisMatch = gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        if(localPlayer.Name == thisMatch.PlayerOName){
            characterPlaying = characterO;
        }else{
            characterPlaying = characterX;
        }
        timeToJump = Random.Range(0.25f, 0.75f);
        characterPlaying = characterO;
        originalY = this.transform.position.y;
        maxY = originalY + 2f;
    }

    void Update(){
        //Check character every second
        timePassed += Time.deltaTime;
        if(timePassed >= 1.0f){
            CheckCharacter();
            timePassed = 0f;
        }
        if(jumping){
            Jump();
        }else
            FSMEnemyBehaviour(Time.deltaTime, characterNear);
    }

    void FSMEnemyBehaviour(float deltaTime, bool charNear){

        switch(actualState){

            case robotState.WALKING:
                //If there is no character near
                if(!charNear){
                    //Follow its path
                    GotoNextPoint();
                }else{
                    //actualState = robotState.ATTACKING;
                    actualState = robotState.JUMPING;
                    Debug.Log("Enter attack mode");
                }
            break;

            case robotState.ATTACKING:
                //If there is character near
                if(charNear){
                    timeAttacking += deltaTime;
                    //If it has been at least 3 seconds attacking, enter rageMode
                    if(timeAttacking <= MAXTIMEATTACKING){
                        //Go to char
                        GoToCharacter();
                    }else{
                        //Enter rageMode
                        actualState = robotState.JUMPING;
                        timeAttacking = 0f;
                        Debug.Log("Jumping time");
                    }
                //If character got away
                }else{
                    actualState = robotState.REROUTE;
                    timeAttacking = 0f;
                }
                
            break;

            case robotState.JUMPING:

                if(characterNear){
                    //If not jumping, reduce time to jump
                    if(!jumping)
                        timeToJump -= deltaTime;
                    //If timeToJump == 0, jump
                    if(timeToJump <= 0){
                        jumping = true;
                        timeToJump = Random.Range(0.25f, 0.75f);
                    //Else goToCharacter
                    }else{
                        GoToCharacter();
                    }
                }else{
                    //Return to route
                    Debug.Log("Gonna walk again");
                    actualState = robotState.REROUTE;
                }
            break;

            case robotState.REROUTE:

                int num = 0;
                float minDistance = float.MaxValue;
                float auxDistance;
                
                //Calculate nearest point
                for(int i = 0; i < points.Length; i++){
                    auxDistance = Vector3.Distance(this.transform.position, points[i].position);
                    if(auxDistance < minDistance){
                        minDistance = auxDistance;
                        num = i;
                    }
                }
                
                //Go to walking with next point
                destPoint = num;
                nextPoint = points[destPoint].transform;
                actualState = robotState.WALKING;

            break;
        }
    }

    void GotoNextPoint(){

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
        //Go to actual point
        if(this.transform.position.x > points[0].position.x && this.transform.position.x < points[1].position.x)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(characterPlaying.transform.position.x, this.transform.position.y, characterPlaying.transform.position.z), Time.deltaTime * speed);
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, characterPlaying.transform.position.y, characterPlaying.transform.position.z), Time.deltaTime * speed);

    }

    void Jump(){
        //Simulate jump
        if(jumpUp){
            if(this.transform.position.y <= maxY)
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + new Vector3(0, maxY, 0), Time.deltaTime * jumpSpeed);
            else
                jumpUp = false;
        }else{
            if(this.transform.position.y > originalY)
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, originalY, this.transform.position.z), Time.deltaTime * jumpSpeed);
            else{
                jumpUp = true;
                jumping = false;
            }
        }
    }

    void CheckCharacter(){
        if(Vector3.Distance(this.transform.position, characterPlaying.transform.position) < 10)
            characterNear = true;
        else
            characterNear = false;
    }
}