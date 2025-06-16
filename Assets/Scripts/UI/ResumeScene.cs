using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeScene : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject flashlight;
    [SerializeField] KeyCode pauseButton;


    public static bool isPaused = false;
 

    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
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
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        flashlight.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        flashlight.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

