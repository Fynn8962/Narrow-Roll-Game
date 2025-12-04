using UnityEngine;

public class StartLine : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null )
        {
            player.StartTimer();
            
        }
    }
}
