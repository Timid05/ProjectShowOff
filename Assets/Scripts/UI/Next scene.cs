using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextscene : MonoBehaviour
{
    public void ToNextScene()
    {
        Debug.Log("Loading scene");
        SceneManager.LoadScene("Comic");
    }
}
