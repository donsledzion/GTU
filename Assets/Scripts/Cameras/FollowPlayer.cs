using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerPos;
    public Vector3 cameraOffset;
    public bool lockRotation = true;
    public GameObject mountPoint;
    [SerializeField] float playerSpeed;
    private Vector3 prevPos;
    private Vector3 curPos;
    [SerializeField] bool dynamicOffset;
    [SerializeField] float minDistance, maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;

        prevPos = playerPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        curPos = playerPos.position;
        playerSpeed = Mathf.Abs((curPos - prevPos).magnitude)/Time.deltaTime;
        prevPos = curPos;

        if (dynamicOffset)
            cameraOffset.y = Mathf.Clamp(minDistance + playerSpeed, minDistance,maxDistance);

        if (mountPoint != null)
            transform.position = mountPoint.transform.position + cameraOffset;
        else
            transform.position = playerPos.position + cameraOffset;
        if (!lockRotation)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerPos.eulerAngles.y, playerPos.eulerAngles.z);
        
    }
}
