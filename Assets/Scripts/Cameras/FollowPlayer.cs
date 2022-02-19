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


    [SerializeField] float prevSpeed;
    [SerializeField] float deltaSpeed;
    [SerializeField] Vector3 interpolateFrom = new Vector3(-1, -1, -1);
    [SerializeField] Vector3 interpolateTo = new Vector3(-1, -1, -1);
    [SerializeField] bool adjustingCameraHeight = false;

    public int interpolationFramesCount = 245; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;

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
        deltaSpeed = Mathf.Abs(playerSpeed - prevSpeed);
        prevSpeed = playerSpeed;
        prevPos = curPos;

        if (dynamicOffset)
        //cameraOffset.y = Mathf.Clamp(minDistance + playerSpeed, minDistance,maxDistance);
        {
            if (!adjustingCameraHeight)
            {
                if (deltaSpeed > .1f)
                {
                    if (interpolateFrom.y < 0)
                        interpolateFrom = cameraOffset;
                }
                if (deltaSpeed < .001f)
                {
                    if (interpolateFrom.y >= 0)
                    {
                        interpolateTo = new Vector3(0, Mathf.Clamp(minDistance + playerSpeed, minDistance, maxDistance), 0);
                        adjustingCameraHeight = true;
                    }
                }
            }
            AdjustCameraHeight();
        }

        if (mountPoint != null)
            transform.position = mountPoint.transform.position + cameraOffset;
        else
            transform.position = playerPos.position + cameraOffset;
        if (!lockRotation)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerPos.eulerAngles.y, playerPos.eulerAngles.z);
        
    }

    void AdjustCameraHeight()
    {
        if (interpolateFrom.y < 0 || interpolateTo.y < 0) return;
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        cameraOffset = Vector3.Lerp(interpolateFrom,interpolateTo,interpolationRatio);

        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)

        if (interpolationRatio >= 1f)
        {
            interpolateFrom = new Vector3(-1, -1, -1);
            interpolateTo = new Vector3(-1, -1, -1);
            adjustingCameraHeight = false;
        }
    }
}
