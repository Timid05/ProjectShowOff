using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LoadingScreen : MonoBehaviour
{
    public static Action<string> OnLoadingScene;

    [SerializeField]
    Image loadingScreen;

    private void OnEnable()
    {
        loadingScreen.gameObject.SetActive(false);  
        OnLoadingScene += EnableLoadingScreen;
    }

    private void OnDisable()
    {
        OnLoadingScene -= EnableLoadingScreen;
    }

    public void EnableLoadingScreen(string name)
    {
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(Loader(name));
    }

    IEnumerator Loader(string scene)
    {
        Time.timeScale = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;


        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3f);

        asyncLoad.allowSceneActivation = true;
    }
}
