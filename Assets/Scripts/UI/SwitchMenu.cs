using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    public GameObject objectToHide;
    public GameObject objectToShow;

    public void SwitchToMenu()
    {
        if (objectToHide != null)
            objectToHide.SetActive(false);

        if (objectToShow != null)
            objectToShow.SetActive(true);
    }
}
