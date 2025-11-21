using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Properties
    [SerializeField] private Transform cameraTransform; //Reference to the camera position
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldFaceMoveDirection = false;

    //Private properties
    private CharacterController controller;
    private Vector3 moveInput;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        controller.Move(moveDirection * speed * Time.deltaTime);

        //Update rotation of the player based on the move direction
        if (shouldFaceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        /*Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.deltaTime);*/

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
