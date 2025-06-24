using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartComicCameraOperator : MonoBehaviour
{
    CinemachineBrain brain;
    [SerializeField]
    SceneAsset mainScene;
    [SerializeField]
    GameObject[] cameras;
    [SerializeField]
    float transitionTime = 1f;
    [SerializeField]
    float waitTime = 1f;
    int currentCamera = 0;
    float lastSwitchTime = 0f;

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = transitionTime;
        if (cameras.Length > 0)
        {
            cameras[0].SetActive(true);
        }
    }

    private void Update()
    {
        if (Time.time - lastSwitchTime >= waitTime)
        {
            if (currentCamera == cameras.Length - 1)
            {
                MoveToMainScene();
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
        SceneManager.LoadScene(mainScene.name);
    }
}
