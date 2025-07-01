using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartComicCameraOperator : MonoBehaviour
{
    CinemachineBrain brain;
    [SerializeField]
    string mainSceneName, menuSceneName;
    [SerializeField]
    GameObject[] cameras;
    [SerializeField]
    float transitionTime = 1f;
    [SerializeField]
    float waitTime = 1f;
    [SerializeField]
    bool isStartComic = true;
    int currentCamera = 0;
    float lastSwitchTime = 0f;
    bool running = false;
    float startTime = 0f;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = transitionTime;
        if (cameras.Length > 0)
        {
            cameras[0].SetActive(true);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }


    void SceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        running = true;
        startTime = Time.time;
    }

    private void Update()
    {
        if (lastSwitchTime == 0f && Time.time - startTime >= waitTime && running)
        {
            cameras[currentCamera].SetActive(false);
            cameras[currentCamera + 1].SetActive(true);
            lastSwitchTime = Time.time;
            currentCamera++;
            return;
        }

        if (lastSwitchTime != 0f && Time.time - lastSwitchTime >= waitTime && running)
        {
            if (currentCamera == cameras.Length - 1)
            {
                LoadingScreen.OnLoadingScene?.Invoke();
                if (isStartComic)
                {
                    MoveToMainScene();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    MoveToMainMenu();
                }
                return;
            }
            cameras[currentCamera].SetActive(false);
            cameras[currentCamera + 1].SetActive(true);
            lastSwitchTime = Time.time;
            currentCamera++;
        }
    }

    void MoveToMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    void MoveToMainMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
