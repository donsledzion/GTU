using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed;
    public float horsePower;
    public float turnSpeed;
    public float verticalInput;
    public float horizontalInput;

    public bool playerDrives;

    public bool handbrake;

    public Rigidbody carRb;

    public Wheel[] wheels;

    public GameObject sterringWheel;
    public Transform driverSeat;

    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        driverSeat = transform.Find("DriverSeat");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (driverSeat.Find("Player"))
            playerDrives = true;
        else
            playerDrives = false;


        if (playerDrives)
            ReadInputs();
        foreach (Wheel wheel in wheels)
        {
            wheel.Accelerate(verticalInput * horsePower);
            wheel.Steer(horizontalInput * turnSpeed);
            wheel.Break(handbrake);
                
            wheel.UpdatePosition();
        }

        RotateSterringWheel(horizontalInput);
    }

    void RotateSterringWheel(float input)
    {
        float tiltAroundZ = input* tiltAngle;
        Quaternion target = Quaternion.Euler(sterringWheel.transform.rotation.x, 0, -tiltAroundZ);
        sterringWheel.transform.rotation = Quaternion.Slerp(sterringWheel.transform.rotation, target, Time.deltaTime * smooth);
    }

    private void ReadInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        handbrake = false;
        if (Input.GetKey(KeyCode.Space))
            handbrake = true;
    }
    
}
