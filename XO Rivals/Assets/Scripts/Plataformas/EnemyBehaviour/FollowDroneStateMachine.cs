using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

//Enum of different states
public enum droneState{WALKING, ATTACKING, RAGING, REROUTE}

public class FollowDroneStateMachine : MonoBehaviour{
    
    //Character GameObject
    [SerializeField] private GameObject characterO;
    [SerializeField] private GameObject characterX;
    private GameObject characterPlaying;
    
    //Game variables
    private GameManager gameState;
    private Match thisMatch;
    private PlayerInfo localPlayer;    

    //State
    droneState actualState = droneState.WALKING;

    //Parameters
    float timeAttacking = 0f;
    private const float MAXTIMEATTACKING = 3f;
    float speed;
    private const float BASESPEED = 2.5f;
    private const float RAGESPEED = 3f;
    bool characterNear = false;

    //Pathfinding route
    [SerializeField] Transform[] points;
    private Transform nextPoint;
    private int destPoint = 0;
    private float timePassed = 0f;

    void Start(){
        nextPoint = points[0];
        speed = BASESPEED;

        gameState = FindObjectOfType<GameManager>();
        thisMatch = gameState.PlayerMatches[PhotonNetwork.CurrentRoom.Name];
        localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        if(localPlayer.Name == thisMatch.PlayerOName){
            characterPlaying = characterO;
        }else{
            characterPlaying = characterX;
        }
        characterPlaying = characterO;
    }

    void Update(){
        //Check character every second
        timePassed += Time.deltaTime;
        if(timePassed >= 1.0f){
            CheckCharacter();
            timePassed = 0f;
        }
        FSMEnemyBehaviour(Time.deltaTime, characterNear);
    }

    void FSMEnemyBehaviour(float deltaTime, bool charNear){

        switch(actualState){

            case droneState.WALKING:
                //If there is no character near
                if(!charNear){
                    //Follow its path
                    GotoNextPoint();
                }else{
                    actualState = droneState.ATTACKING;
                    Debug.Log("Enter attack mode");
                }
            break;

            case droneState.ATTACKING:
                //If there is character near
                if(charNear){
                    timeAttacking += deltaTime;
                    //If it has been at least 3 seconds attacking, enter rageMode
                    if(timeAttacking <= MAXTIMEATTACKING){
                        //Go to char
                        GoToCharacter();
                    }else{
                        //Enter rageMode
                        actualState = droneState.RAGING;
                        speed = RAGESPEED;
                        timeAttacking = 0f;
                        Debug.Log("Enrage");
                    }
                //If character got away
                }else{
                    actualState = droneState.REROUTE;
                    timeAttacking = 0f;
                }
                
            break;

            case droneState.RAGING:

                if(characterNear){
                    //Go to char, but faster
                    GoToCharacter();
                }else{
                    //Return to route and slow down
                    Debug.Log("Gonna walk again");
                    actualState = droneState.REROUTE;
                    speed = BASESPEED;
                }
            break;

            case droneState.REROUTE:

                int num = 0;
                float minDistance = float.MaxValue;
                float auxDistance;
                
                //Calculate nearest point
                for(int i = 0; i < points.Length; i++){
                    auxDistance = Vector3.Distance(this.transform.position, points[i].transform.position);
                    if(auxDistance < minDistance){
                        minDistance = auxDistance;
                        num = i;
                    }
                }
                
                //Go to walking with next point
                destPoint = num;
                nextPoint = points[destPoint];
                actualState = droneState.WALKING;

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
            this.transform.position = Vector3.MoveTowards(this.transform.position, characterPlaying.transform.position, Time.deltaTime * speed);
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, characterPlaying.transform.position.y, characterPlaying.transform.position.z), Time.deltaTime * speed);
    }

    void CheckCharacter(){
        if(Vector3.Distance(this.transform.position, characterPlaying.transform.position) < 10)
            characterNear = true;
        else
            characterNear = false;
    }
}