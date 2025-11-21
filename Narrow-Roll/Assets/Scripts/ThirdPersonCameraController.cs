using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    //Properties
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f; //Smoothness
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 15f;

    //Private properties
    private PlayerControls controls;

    private CinemachineCamera cam;
    private CinemachineOrbitalFollow orbital;
    private Vector2 scrollDelta;

    private float targetZoom;
    private float currentZoom;

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.CameraControls.MouseZoom.performed += MandleMouseScroll; //if mousewheel pressed --> call this function
        
        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();

        targetZoom = currentZoom = orbital.Radius;
    }

    private void MandleMouseScroll(InputAction.CallbackContext context)
    {
        scrollDelta = context.ReadValue<Vector2>();
        Debug.Log($"Mouse is scrolling. Value: {scrollDelta}");
    }

    void Update()
    {
        if (scrollDelta.y != 0)
        {
            if (orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDistance, maxDistance); // keep targetZoom between min and max distances
                scrollDelta = Vector2.zero; 

            }
        }


        //Controller Zoom Input
        float bumperDelta = controls.CameraControls.GamepadZoom.ReadValue<float>();
        if (bumperDelta != 0)
        {
            Mathf.Clamp(orbital.Radius - bumperDelta * zoomSpeed, minDistance, maxDistance);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);//smooth out the zoom
        orbital.Radius = currentZoom;
    }

    
}
