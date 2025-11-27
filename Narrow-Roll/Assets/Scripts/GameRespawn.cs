using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    [SerializeField] private float respawnThresholdY = -10f; // Y value when player gets reset after falling
    [SerializeField] private Vector3 respawnPosition = new Vector3(0f, 6f, 0f); // Respawn position

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();


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
        // Reset position
        transform.position = respawnPosition;

        // Reset all velocities
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;  
            rb.angularVelocity = Vector3.zero; 
        }

        
        transform.rotation = Quaternion.identity;

        Debug.Log("Player wurde zurückgesetzt!");
    }
}