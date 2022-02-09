using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    public Vector3 cameraOffset;
    public bool lockRotation = true;
    public GameObject mountPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (mountPoint != null)
            transform.position = mountPoint.transform.position + cameraOffset;
        else
            transform.position = player.transform.position + cameraOffset;
        if (!lockRotation)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, player.transform.eulerAngles.y, player.transform.eulerAngles.z);
        
    }
}
