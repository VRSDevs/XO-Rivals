using UnityEngine;

public class ViewCone : MonoBehaviour {

    [SerializeField] private ShootDroneBehaviourTree parent;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
            parent.CharacterDetected(true);
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
            parent.CharacterDetected(false);
    }
}