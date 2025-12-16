using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Cinemachine;
using Cursor = UnityEngine.Cursor;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private VisualElement ui;
    private Button continueButton;
    private Button restartButton;
    private Button quitButton;

    [Header("Referenzen")] 
    public GameObject cameraObjekt;

    private CinemachineInputAxisController cameraInput;
    private GameRespawn respawnScript;
    private TimerManager timerManager;
    private PlayerController playerController; // Player Script
    private PlayerControls playerControls; // input Actions

    
    private float defaultTimeScale = 1f;
    private bool isRestarting = false;
    private bool isPaused = false;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        cameraInput = cameraObjekt.GetComponent<CinemachineInputAxisController>();

        respawnScript = FindFirstObjectByType<GameRespawn>();
        timerManager = FindFirstObjectByType<TimerManager>();
        playerController = FindFirstObjectByType<PlayerController>();
        playerControls = new PlayerControls();

        playerControls.UI.Pause.performed += ctx => TogglePause();

        // Disable menu
        if (ui != null) ui.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        playerControls.Enable();


    }

    private void OnDisable()
    {
        playerControls.Disable();
        
    }


    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;

        if (ui != null) ui.style.display = DisplayStyle.Flex;

        // Pause game physic
        Time.timeScale = 0f;

        // Unlock Mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause camera input
        if (cameraInput != null)
        {
            cameraInput.enabled = false;
        }

        // Timer pausieren
        if (timerManager != null)
        {
            timerManager.PauseTimer();
        }

        BindButtons();
    }

    private void ResumeGame()
    {
        isPaused = false;

        if (ui != null) ui.style.display = DisplayStyle.None;


        Time.timeScale = defaultTimeScale;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraInput != null) cameraInput.enabled = true;

        if (timerManager != null && !isRestarting)
        {
            timerManager.ResumeTimer();
        }

        if (continueButton != null) continueButton.clicked -= OnContinueButtonClicked;
        if (restartButton != null) restartButton.clicked -= OnRestartButtonClicked;
        if (quitButton != null) quitButton.clicked -= OnQuitButtonClicked;
    }

    private void BindButtons()
    {
        if (ui == null) return;

        continueButton = ui.Q<Button>("PlayButton");
        if (continueButton != null)
        {
            continueButton.clicked -= OnContinueButtonClicked; // Remove first to prevent double clicks
            continueButton.clicked += OnContinueButtonClicked;
        }

        restartButton = ui.Q<Button>("RestartButton");
        if (restartButton != null)
        {
            restartButton.clicked -= OnRestartButtonClicked;
            restartButton.clicked += OnRestartButtonClicked;
        }

        quitButton = ui.Q<Button>("QuitButton");
        if (quitButton != null)
        {
            quitButton.clicked -= OnQuitButtonClicked;
            quitButton.clicked += OnQuitButtonClicked;
        }
    }




    private void OnContinueButtonClicked()
    {
        ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
        isRestarting = false;
        isPaused = false;
        if (ui != null) ui.style.display = DisplayStyle.None;
        Time.timeScale = defaultTimeScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraInput != null) cameraInput.enabled = true;

        if (respawnScript != null)
        {
            respawnScript.RespawnPlayer();
        }

        if (timerManager != null)
        {
            timerManager.ResetTimer();
        }
        

    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
