using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    //Properties
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f; //Smoothness
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 15f;

    [Header("Respawn Settings")]
    [SerializeField] private float respawnCameraDistance = 10f; // Standard-Kameradistanz bei Respawn
    [SerializeField] private float respawnHorizontalAngle = 0f;
    [SerializeField] private float respawnVerticalAngle = 0.3f;

    //Private properties
    private PlayerControls controls;
    private CinemachineCamera cam;
    private CinemachineOrbitalFollow orbital;
    private Vector2 scrollDelta;
    private float targetZoom;
    private float currentZoom;
    private Transform playerTransform;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.CameraControls.MouseZoom.performed += HandleMouseScroll; //if mousewheel pressed --> call this function

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();
        targetZoom = currentZoom = orbital.Radius;

        // Finde den Player (das Follow-Target der Kamera)
        if (cam.Follow != null)
        {
            playerTransform = cam.Follow;
            lastPlayerPosition = playerTransform.position;
        }
    }

    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;

        scrollDelta = context.ReadValue<Vector2>();
       
    }

    void Update()
    {   
        if (Time.timeScale == 0) return;

        // Check if player was reseted
        if (playerTransform != null)
        {
            float distanceMoved = Vector3.Distance(playerTransform.position, lastPlayerPosition);

            
            if (distanceMoved > 20f)
            {
                ResetCameraOnRespawn();
            }

            lastPlayerPosition = playerTransform.position;
        }

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
            targetZoom = Mathf.Clamp(orbital.Radius - bumperDelta * zoomSpeed, minDistance, maxDistance);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);//smooth out the zoom
        orbital.Radius = currentZoom;
    }

    private void ResetCameraOnRespawn()
    {
        // Reset camera Values
        targetZoom = respawnCameraDistance;
        currentZoom = respawnCameraDistance;
        orbital.Radius = respawnCameraDistance;

        // Reset Rotation
        orbital.HorizontalAxis.Value = respawnHorizontalAngle;
        orbital.VerticalAxis.Value = respawnVerticalAngle;

    }

    public void ManualResetCamera()
    {
        ResetCameraOnRespawn();
    }

    void OnDestroy()
    {
        if (controls != null)
        {
            controls.CameraControls.MouseZoom.performed -= HandleMouseScroll;
            controls.Disable();
        }
    }
}