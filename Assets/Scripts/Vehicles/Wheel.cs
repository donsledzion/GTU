using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] bool powered;
    [SerializeField] bool handbrakes;
    [SerializeField] float maxAngle = 30f;
    [SerializeField] float offset = 0f;

    private float turnAngle;
    private WheelCollider wheelCollider;
    private Transform wheelMesh;

    // Start is called before the first frame update
    void Start()
    {
        wheelCollider = transform.Find("wheelCollider").GetComponent<WheelCollider>();
        wheelMesh = transform.Find("wheelMesh");
    }

    public void Steer(float steerInput)
    {
        turnAngle = steerInput * maxAngle + offset;
        wheelCollider.steerAngle = turnAngle;
    }

    public void Accelerate(float powerInput)
    {
        if (powered)
            wheelCollider.motorTorque = powerInput;
        else
            wheelCollider.brakeTorque = 0;
    }

    public void Break(bool handbrake)
    {
        if (handbrake && handbrakes)
            wheelCollider.brakeTorque = 99999;
        else
            wheelCollider.brakeTorque = 0;

    }

    public void UpdatePosition()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }
}
