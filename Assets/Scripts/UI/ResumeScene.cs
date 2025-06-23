using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeScene : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] KeyCode pauseButton;


    public static bool isPaused = false;
 

    void Update()
    {
        if (Input.GetKeyDown(pauseButton) && !GameStateActions.inDialogue)
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

        if (isPaused && Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


void PauseGame()
    {
        GameStateActions.OnGamePause?.Invoke(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        GameStateActions.OnGamePause?.Invoke(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

