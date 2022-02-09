using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float moveSpeed = 16;
    public float rotationSpeed = 140;
    public Animator animator;
    public Rigidbody playerRb;
    public Collider collisionCollider;


    // Jumping section variables
    public float jumpTime = 0.75f;
    public bool jumping = false;
    private float jumpTimeStart;

    public float punchTime = 0.1f;
    public float punchedAt;
    public float punchForce;

    public List<Rigidbody> otherRbs;
    public Transform carExitSpot;
    public bool inTheCar;
    public GameObject[] nearbyCars;
    //public GameObject nearestCar;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        jumping = true;
        jumpTimeStart = Time.time;
        animator.SetTrigger("Jump_trig");
    }

    public void MoveAround(float inputH, float inputV)
    {
        float direction = inputH * Time.deltaTime * rotationSpeed;
        transform.Rotate(Vector3.up, direction);
        transform.Translate(Vector3.forward * inputV * Time.deltaTime * moveSpeed);

        animator.SetBool("Static_b", true);
        animator.SetFloat("Speed_f", inputV);
    }

    public void HandleJumping()
    {
        if (jumping)
        {
            float deltaTime = Time.time - jumpTimeStart;
            if (deltaTime > jumpTime)
            {
                jumping = false;
                return;
            }
            playerRb.AddRelativeForce(Vector3.forward * 4000, ForceMode.Force);
        }
    }

    public void Fire()
    {
        StartCoroutine(PunchCooldown());
    }


    IEnumerator PunchCooldown()
    {
        HandlePunch();
        animator.speed = 2;
        animator.SetInteger("WeaponType_int", 1);
        animator.SetBool("Shoot_b", false);
        animator.SetBool("Reload_b", false);
        punchedAt = Time.time;
        while (Time.time - punchedAt < punchTime)
        {
            yield return null;
        }
        animator.speed = 1;
        animator.SetInteger("WeaponType_int", 0);
    }

    void HandlePunch()
    {
        foreach (Rigidbody otherRb in otherRbs)
        {
            Vector3 hitDirection = (otherRb.gameObject.transform.position - transform.position).normalized;
            otherRb.AddForce(hitDirection * punchForce, ForceMode.Impulse);
        }

    }

    public void ToggleCar()
    {
        if (!inTheCar)
        {
            GetInTheCar(NearestCar());
        }
        else
        {
            LeaveTheCar();
            Debug.Log("You are in the car!!");
        }
    }

    private void GetInTheCar(GameObject drivableCar)
    {
        collisionCollider.isTrigger = true;
        playerRb.isKinematic = true;
        playerRb.useGravity = false;
        playerRb.constraints = RigidbodyConstraints.FreezePosition;
        carExitSpot = drivableCar.transform.Find("ExitSpot").transform;
        transform.SetParent(drivableCar.transform.Find("DriverSeat").transform, true);
        transform.localPosition = Vector3.zero;
        transform.rotation = new Quaternion();
        inTheCar = true; animator.SetFloat("Speed_f", 0f);
    }

    private void LeaveTheCar()
    {
        transform.parent = null;
        transform.position = carExitSpot.position;
        carExitSpot = null;
        playerRb.constraints = RigidbodyConstraints.None;
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        collisionCollider.isTrigger = false;
        playerRb.isKinematic = false;
        playerRb.useGravity = true;
        inTheCar = false;
    }

    private GameObject NearestCar()
    {
        if (nearbyCars.Length > 0)
        {
            GameObject nearestCar = nearbyCars[0];
            float shortestDistance = (nearestCar.transform.position - transform.position).magnitude;
            foreach (GameObject car in nearbyCars)
            {
                float distanceToCar = (car.transform.position - transform.position).magnitude;
                if (distanceToCar < shortestDistance)
                {
                    nearestCar = car;
                    shortestDistance = distanceToCar;
                }
            }
            return nearestCar;
        }
        return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RandomGuy"))
            otherRbs.Add(other.GetComponent<Rigidbody>());
    }

    public void ScanArea(float radius) //TODO - handle scanning area for special events and objects
    {
        nearbyCars = GameObject.FindGameObjectsWithTag("DrivableCar");
    }

    private void OnTriggerExit(Collider other)
    {
        try
        {
            otherRbs.Remove(other.GetComponent<Rigidbody>());
        }
        catch
        {
            Debug.Log("No such a rigidbody in registry dude...");
        }

    }
}
