using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisibility : MonoBehaviour
{
    [SerializeField] KeyCode mapButton;

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount != 0)
        {
            // Map is visible if the player holds down the map button
            if (Input.GetKeyDown(mapButton)) { ChangeChildrenVisibility(true); }
            if (Input.GetKeyUp(mapButton)) { ChangeChildrenVisibility(false); }
        }

        //if (canvas != null)
        //{
        //    // Map is visible if the player holds down the map button
        //    if (Input.GetKeyDown(mapButton)) { canvas.enabled = true; }
        //    if (Input.GetKeyUp(mapButton)) { canvas.enabled = false; }
        //}
    }


    void ChangeChildrenVisibility(bool newVisibility)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(newVisibility);
        }
    }
}
