using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LoadingScreen : MonoBehaviour
{
    public static Action OnLoadingScene;

    [SerializeField]
    Image loadingScreen;

    private void OnEnable()
    {
        loadingScreen.enabled = false;
        OnLoadingScene += EnableLoadingScreen;
    }

    private void OnDisable()
    {
        OnLoadingScene -= EnableLoadingScreen;
    }

    public void EnableLoadingScreen()
    {
        loadingScreen.enabled = true;
    }
}
