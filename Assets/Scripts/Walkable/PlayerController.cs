using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class PlayerController : MonoBehaviour
{
    Human player;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Human>();
    }

    void Update()
    {
        player.ScanArea(0);

        if (Input.GetKeyDown(KeyCode.Return))
            player.ToggleCar();

        if (!player.inTheCar)
        {
            player.MoveAround(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.LeftControl))
                player.Fire();

            if (Input.GetKeyDown(KeyCode.Space))
                player.Jump();
            player.HandleJumping();
        } else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }   
    }

}
