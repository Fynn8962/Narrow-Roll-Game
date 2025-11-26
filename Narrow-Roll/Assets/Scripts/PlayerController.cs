using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform cameraTransform; // Reference to the camera position
    [SerializeField] private float moveForce = 125f; // movement force
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float airControl = 0.3f; // Control in the air (0-1)

    [Header("Physics")]
    [SerializeField] private float sphereMass = 5f;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float angularDrag = 1.2f;
    [SerializeField] private float gravityMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.6f;

 


    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody Setup for realistic sphere
        rb.mass = sphereMass;
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        //for realistic roll-physic
        rb.freezeRotation = false;

  
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        CheckGround();
        rb.AddForce(Physics.gravity * (gravityMultiplier - 1f) * rb.mass, ForceMode.Force);
        HandleMovement();
    }

    void CheckGround()
    {
        //for better ground detection
        isGrounded = Physics.SphereCast(
            transform.position,
            0.4f,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer
        );
    }

    void HandleMovement()
    {
        //movement based on camera direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            float currentMoveForce = isGrounded ? moveForce : moveForce * airControl;

            // max. speed
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (horizontalVelocity.magnitude < maxSpeed)
            {
                //Torque for realistic roll-physic
                Vector3 torqueDirection = Vector3.Cross(Vector3.up, moveDirection);
                rb.AddTorque(torqueDirection * currentMoveForce * 0.4f, ForceMode.Force);

                //additional force for better control
                rb.AddForce(moveDirection * currentMoveForce * 1.2f, ForceMode.Force);
            }
        }
    }



            // Visualisierung im Editor
            void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, 0.4f);
    }
}




