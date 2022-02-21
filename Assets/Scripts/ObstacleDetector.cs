using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField] float scanRange = 5f;
    [SerializeField] SphereCollider obstacleDetector;

    [SerializeField] public GameObject objectInFront;

    [SerializeField] string navigationHint = "";
    [SerializeField] public bool obstacleOnLeftHand, obstacleOnRightHand;
    [SerializeField] LayerMask collisionMask;

    void Start()
    {
        if (obstacleDetector == null)
            obstacleDetector = gameObject.GetComponent<SphereCollider>();
    }

    void Update()
    {
        DetectObstacle();
    } 

    void DetectObstacle()
    {
        RaycastHit hit;
        Vector3 capsuleBottom = new Vector3(transform.position.x, 0.7f, transform.position.z);
        Vector3 capsuleTop = new Vector3(transform.position.x, 1.5f, transform.position.z);
        float capsuleRadius = .65f;
        if (Physics.CapsuleCast(capsuleBottom,capsuleTop,capsuleRadius, transform.forward,out hit, scanRange,collisionMask))
        {
            objectInFront = hit.transform.gameObject;
            HandleObstacle();
        }
        else
        {
            objectInFront = null;

            obstacleOnLeftHand = false;
            obstacleOnRightHand = false;
            navigationHint = "";
        }
            
    }

    void HandleObstacle()
    {
        float angleToObstacle = Vector3.SignedAngle(objectInFront.transform.position - transform.position, transform.forward, Vector3.up);
        if (angleToObstacle < 0f)
        {
            navigationHint = "Obstacle is on your right";
            obstacleOnLeftHand = false;
            obstacleOnRightHand = true;
        }
        else if (angleToObstacle > 0f)
        {
            navigationHint = "Obstacle is on your left";
            obstacleOnLeftHand = true;
            obstacleOnRightHand = false;
        }
        else
        {
            navigationHint = "Obstacle is directly in front";
            obstacleOnLeftHand = true;
            obstacleOnRightHand = false;
        } 
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, .1f);

        if (objectInFront != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(objectInFront.transform.position, 1f);
        }

    }
}
