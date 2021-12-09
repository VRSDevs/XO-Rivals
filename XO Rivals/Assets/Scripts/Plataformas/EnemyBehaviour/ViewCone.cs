using UnityEngine;

public class ViewCone : MonoBehaviour {

    [SerializeField] private ShootDroneBehaviourTree parent;

    public void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("")){
            parent.CharacterDetected();
        }
    }
}