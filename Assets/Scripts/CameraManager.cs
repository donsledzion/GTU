using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera[] cameras;
    int currentCameraIndex;
    Camera currentCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (cameras.Length > 0)
        {
            if (cameras.Length > 1)
                foreach (Camera camera in cameras)
                    camera.gameObject.SetActive(false);

            currentCameraIndex = 0;
            currentCamera = cameras[currentCameraIndex];
            currentCamera.gameObject.SetActive(true);
            
        }
        else
            Debug.LogError("Seems like there is no cameras assigned to Camera Manager.");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleCamera();
    }

    void ToggleCamera()
    {
        if (cameras.Length > 1)
            if (currentCameraIndex < cameras.Length - 1)
                currentCameraIndex++;
        else if(currentCameraIndex == cameras.Length-1)
            currentCameraIndex = 0;
        currentCamera.gameObject.SetActive(false);
        currentCamera = cameras[currentCameraIndex];
        currentCamera.gameObject.SetActive(true);

    }
}
