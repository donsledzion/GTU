using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby : Human
{
    [SerializeField]GameObject target;
    [SerializeField] public Transform[] targetSet;
    [SerializeField] bool loopTargets;
    [SerializeField] float angleBetweenVectors;
    [SerializeField] bool isColliding;
    [Range(0.3f, 1.0f)]
    [SerializeField] public float speedFactor = 0.5f;
    private ObstacleDetector obstacleDetector;
    [SerializeField] float avoidAngleStep = 1f;

    int currentTarget = 0;

    private void Awake()
    {
        obstacleDetector = GetComponent<ObstacleDetector>();
    }

    void Update()
    {
        if(target)
            NavigateToTarget(target.transform);
        if (targetSet.Length > 0)
            NavigateThroughTargets(targetSet);
    }

    bool NavigateToTarget(Transform targetTransform)
    {
        Vector3 targetDirection = (targetTransform.position - transform.position);
        Vector2 stragihtToTarget = Vector2.zero;
        if (targetDirection.magnitude < .5f)
        {
            target = null;
            MoveAround(stragihtToTarget.x, stragihtToTarget.y);
            return true;
        }
        else
        {
            angleBetweenVectors = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            stragihtToTarget = new Vector2(angleBetweenVectors / Mathf.Abs(angleBetweenVectors), speedFactor);
        }
        if(obstacleDetector.objectInFront && DistanceTo(obstacleDetector.objectInFront.transform)< DistanceTo(targetSet[currentTarget]))
        {
            float directionSign = 1f;
            if (obstacleDetector.obstacleOnRightHand)
                directionSign = -1f;
            gameObject.transform.Rotate(Vector3.up, avoidAngleStep * directionSign);
        }
        if (!isBetween(angleBetweenVectors, -60f, 60f)) stragihtToTarget.y/=3;
        MoveAround(stragihtToTarget.x, stragihtToTarget.y);
        return false;
    }

    void NavigateThroughTargets(Transform[] targets)
    {
        if (targets.Length < 1) return;

        if (targets.Length == 1) NavigateToTarget(targets[0]);
        else if (currentTarget < targets.Length)
        {
            if (NavigateToTarget(targets[currentTarget])) currentTarget++;
        }
        else if (loopTargets) currentTarget = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    float DistanceTo(Transform _transform)
    {
        return (_transform.position - gameObject.transform.position).magnitude;
    }

    public static bool isBetween(float value, float firstBound, float secondBound)
    {
        float lowerBound = firstBound;
        float higherBound = secondBound;

        if(firstBound > secondBound)
        {
            lowerBound = secondBound;
            higherBound = firstBound;
        }
        return (value>=lowerBound && value <= higherBound);
    }
}
