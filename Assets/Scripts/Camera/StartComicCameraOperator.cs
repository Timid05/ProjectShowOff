using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartComicCameraOperator : MonoBehaviour
{
    [SerializeField]
    UDictionary<GameObject, AudioClip> camsToAudio = new UDictionary<GameObject, AudioClip>();
    CinemachineBrain brain;
    [SerializeField]
    string mainSceneName, menuSceneName;
    [SerializeField]
    GameObject[] cameras;
    [SerializeField]
    float transitionTime = 1f;
    [SerializeField]
    float additionalWaitTime = 1f;
    [SerializeField]
    bool isStartComic = true;

    int currentCamera = 0;
    float lastSwitchTime = 0f;
    bool running = false;
    float startTime = 0f;
    bool loading = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        brain = GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = transitionTime;
        if (cameras.Length > 0)
        {
            cameras[0].SetActive(true);
            SubtitleHandler.OnPlayAudioWithSubtitles?.Invoke(camsToAudio[cameras[0]]);
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

    float GetCurrentClipLength()
    {
        if (camsToAudio.Keys.Contains(cameras[currentCamera]) && camsToAudio[cameras[currentCamera]] != null)
        {
            return camsToAudio[cameras[currentCamera]].length;
        }
        else return 0;
    }

    private void Update()
    {
        if (loading) { return; }
        if (lastSwitchTime == 0f && Time.time - startTime >= GetCurrentClipLength() + additionalWaitTime && running)
        {
            cameras[currentCamera].SetActive(false);
            cameras[currentCamera + 1].SetActive(true);
            lastSwitchTime = Time.time;
            currentCamera++;
            SubtitleHandler.OnPlayAudioWithSubtitles?.Invoke(camsToAudio[cameras[currentCamera]]);
            return;
        }

        if (lastSwitchTime != 0f && Time.time - lastSwitchTime >= GetCurrentClipLength() + additionalWaitTime && running)
        {
            if (currentCamera == cameras.Length - 1)
            {
                if (isStartComic)
                {
                    ControlsPopup.OnShowControls?.Invoke();
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
            SubtitleHandler.OnPlayAudioWithSubtitles?.Invoke(camsToAudio[cameras[currentCamera]]);
        }
    }



    void MoveToMainScene()
    {
        loading = true;
        LoadingScreen.OnLoadingScene?.Invoke("Combining");
    }

    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
