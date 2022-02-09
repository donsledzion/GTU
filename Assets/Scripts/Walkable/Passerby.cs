using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby : Human
{
    [SerializeField]GameObject target;
    [SerializeField] float angleBetweenVectors;
    
    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 targetDirection = (target.transform.position - transform.position);
            if (targetDirection.magnitude < 2.5f)
            {
                target = null;
                MoveAround(0f, 0f);
                return;
            }   

            angleBetweenVectors = Vector3.SignedAngle(transform.forward,targetDirection,Vector3.up);

            Vector2 stragithToTarget = new Vector2(angleBetweenVectors / Mathf.Abs(angleBetweenVectors),0.5f);

            MoveAround(stragithToTarget.x, stragithToTarget.y);

            
        }
    }
}
