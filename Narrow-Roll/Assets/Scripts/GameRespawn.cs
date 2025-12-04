using Unity.VisualScripting;
using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    [SerializeField] private float respawnThresholdY = -35f; // Y value when player gets reset after falling
    [SerializeField] private Vector3 respawnPosition = new Vector3(0f, 3f, 0f); // Respawn position

    private Rigidbody rb;
    private TimerManager timerManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timerManager = GetComponent<TimerManager>();    

    }

    void FixedUpdate()
    {
        if (transform.position.y < respawnThresholdY)
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        timerManager.ResetTimer();

        // Reset all velocities
        if (rb != null)
        {
            // Reset Velocity
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Reset Position and Rotation
            rb.position = respawnPosition;
            rb.rotation = Quaternion.identity;

            // Reset Physics
            rb.Sleep();
            rb.WakeUp();
        }

        Debug.Log("Player wurde zurückgesetzt!");
    }
}