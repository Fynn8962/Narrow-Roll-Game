using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int maxRounds = 1;
    private int currentRound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null )
        {
            player.StopTimer();
            player.IncreaseRound();
            CheckRoundCompletion(player);
        }
    }



    private void CheckRoundCompletion(PlayerController player)
    {
        if(player.currentRound >= maxRounds)
        {
            EndRound(true);
        }
    }

    void EndRound(bool success)
    {
        if(success)
        {
            Debug.Log("Round Finished");
        }
        else
        {
            Debug.Log("Round Finished");
        }
    }
}
