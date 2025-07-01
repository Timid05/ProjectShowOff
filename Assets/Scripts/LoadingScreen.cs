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

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Loading done");

        yield return new WaitForSecondsRealtime(10f);

        SceneManager.UnloadSceneAsync("Comic");
    }
}
