using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float currentTime;
    private bool timerRunning = false;

    private bool timerHasStarted = false;
    private bool hasFinished = false;

    private float bestTime;
    private float finaleTime;

    private void Update()
    {
        if (timerRunning)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        timerHasStarted = true; 
        timerRunning = true;
        hasFinished = false;
        Debug.Log("TM.cs: Timer gestartet");
    }

    public void PauseTimer()
    {
        timerRunning = false;
        Debug.Log("TM.cs: Timer pausiert");
    }

    public void ResumeTimer()
    {
        if (!timerHasStarted || hasFinished) return;

        timerRunning = true;
        Debug.Log("TM.cs: Timer fortgesetzt");



    }

    public void StopTimer()
    {
        if (timerRunning)
        {
            timerRunning = false; 
            hasFinished = true;

            finaleTime = currentTime;

            if (bestTime == 0f || finaleTime < bestTime)
            {
                bestTime = currentTime;
                Debug.Log("TM.cs: Neue Bestzeit");
            }

            Debug.Log("TM.cs:Timer gestoppt! Zeit: " + finaleTime.ToString("F2") + " Sekunden");
        }
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        timerRunning = false;
        timerHasStarted = false;
        hasFinished = false;
        Debug.Log("TM.cs: Timer reset");
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsTimerRunning()
    {
        return timerRunning;
    }


    public float GetBestTime()
    {
        return bestTime;
    }
}
