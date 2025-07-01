using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopup : MonoBehaviour
{
    public static Action OnShowControls;

    private void OnEnable()
    {
        OnShowControls += ShowControls;
    }

    private void OnDisable()
    {
        OnShowControls -= ShowControls;
    }

    public void ShowControls()
    {
        Time.timeScale = 0f;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
