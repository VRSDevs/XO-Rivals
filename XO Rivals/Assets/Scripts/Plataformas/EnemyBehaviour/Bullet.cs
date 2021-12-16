using UnityEngine;

public class Bullet : MonoBehaviour {

    //GameObject that shoots
    [SerializeField] private GameObject parent;
    public GameObject character;

    //Distance to shooter
    private float distanceToParent = 0f;
    private const float MAXDISTTOPARENT = 30f;
    private const float SPEED = 3f;

    void Update(){

        //Move
        this.transform.position = Vector3.MoveTowards(this.transform.position, character.transform.position, Time.deltaTime * SPEED);

        //Update distance to parent
        distanceToParent = Vector3.Distance(this.transform.position, parent.transform.position);

        //If distance is greater than max, hide
        if(distanceToParent >= MAXDISTTOPARENT){
            Destroy(this);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            Destroy(this);
        }
    }
}