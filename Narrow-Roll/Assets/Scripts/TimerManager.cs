using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float startTime;
    private float endTime;
    private float currentTime;
    private bool timerRunning = false;


    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
        Debug.Log("Timer ist gestartet");
    }

    public void StopTimer()
    {
        if (timerRunning)
        {
            endTime = Time.time;
            float elapsedTime = endTime - startTime;
            timerRunning = false;

            Debug.Log("Timer gestoppt! Zeit: " + elapsedTime.ToString("F2") + " Sekunden");
        }
    }

    public void ResetTimer()
    {
        startTime = 0f;
        endTime = 0f;
        currentTime = 0f;
        timerRunning = false;
        Debug.Log("Timer reset");
    }

    public float GetCurrentTime()
    {
        if (timerRunning)
        {
            return Time.time - startTime;
        }
        return 0f;
    }

    public bool IsTimerRunning()
    {
        return timerRunning;
    }

    public float GetLastTime()
    {
        return currentTime;
    }
}
