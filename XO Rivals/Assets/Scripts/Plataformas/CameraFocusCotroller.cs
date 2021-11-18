using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocusCotroller : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField]
    private GameObject playerX;
    [SerializeField]
    private GameObject playerO;

    private PlayerMovement playerOController;
    private PlayerMovement playerXController;

    public List<Transform> points;
    public Transform cameraFollow;
    int goalPoint = 0;
    public float moveSpeed = 1;


    private void Start()
    {
        playerOController = playerO.GetComponent<PlayerMovement>();
        playerXController = playerX.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        MoveToNextPoint();

        if (playerOController.win || playerXController.win || playerOController.isDead || playerXController.isDead)
        {
            cameraFollow.gameObject.SetActive(false);
        }
    }

    void MoveToNextPoint()
    {
        cameraFollow.position = Vector2.MoveTowards(cameraFollow.position, points[goalPoint].position, Time.deltaTime * moveSpeed);
        if (Vector2.Distance(cameraFollow.position, points[goalPoint].position) < 0.1f)
        {
            if (goalPoint == points.Count - 1)
            {
                cameraFollow.position = points[goalPoint].position;
                
            }
                
            else
                goalPoint++;

        }

    }
}

