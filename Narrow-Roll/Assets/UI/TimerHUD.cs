using System;
using UnityEngine;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;

public class TimerHUD : MonoBehaviour
{
    private Label timerLabel; // UI element
    private Label bestTimeLabel;

    private TimerManager timerManager; // datasource

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null) // if no UI element found
        {
            return;
        }

        var root = uiDocument.rootVisualElement;

        root.pickingMode = PickingMode.Ignore; // Ignore all Clicks, Hover and Touch actions. 

        timerLabel = root.Q<Label>("TimerLabel");

        bestTimeLabel = root.Q<Label>("BestTimeLabel");

        timerManager = FindFirstObjectByType<TimerManager>();
    }

    private void Update()
    {
        if (timerLabel == null || timerManager == null) return;

        float time = timerManager.GetCurrentTime();
        timerLabel.text = FormatTime(time);

        if (bestTimeLabel != null)
        {
            float bestTime = timerManager.GetBestTime();

            if (bestTime == 0f)
            {
                bestTimeLabel.text = "Best: --:--";
            }
            else
            {
                bestTimeLabel.text = "Best: " + FormatTime((bestTime));
            }
        }



    }

    private string FormatTime(float timeInSeconds)
    {
        // Format 00:00:00
        float minutes = Mathf.FloorToInt(timeInSeconds / 60);
        float seconds = Mathf.FloorToInt(timeInSeconds % 60);
        float milliseconds = (timeInSeconds % 1) * 100;

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
