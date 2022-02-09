using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby : Human
{
    [SerializeField]GameObject target;
    [SerializeField]Transform[] targetSet;
    [SerializeField] float angleBetweenVectors;
    [SerializeField] bool isColliding;
    [Range(0.3f, 1.0f)]
    [SerializeField] float speedFactor = 0.5f;

    int currentTarget = 0;
        
        
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
        Vector2 stragithToTarget = Vector2.zero;
        if (targetDirection.magnitude < .5f || isColliding)
        {
            target = null;
            MoveAround(stragithToTarget.x, stragithToTarget.y);
            return true;
        }
        else
        {
            angleBetweenVectors = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            stragithToTarget = new Vector2(angleBetweenVectors / Mathf.Abs(angleBetweenVectors), speedFactor);
        }
        MoveAround(stragithToTarget.x, stragithToTarget.y);
        return false;
    }

    void NavigateThroughTargets(Transform[] targets)
    {
        if (targets.Length < 1) return;

        if (targets.Length == 1) NavigateToTarget(targets[0]);
        else if(currentTarget < targets.Length)
            if (NavigateToTarget(targets[currentTarget])) currentTarget++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
