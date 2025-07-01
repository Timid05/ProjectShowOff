using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignClose : MonoBehaviour
{
    public static event Action<GameObject, bool> OnSignPlayerDistanceStatusChange;
    // Sends out a delegate based on whether the player is nearby so that the map sign can be highlighted.

    private void OnTriggerEnter(Collider other)
    {
        if(OnSignPlayerDistanceStatusChange != null && other.gameObject.CompareTag("Player"))
        {
            Debug.LogFormat("Player enter sign {0} highlight zone.", gameObject.name);
            OnSignPlayerDistanceStatusChange(gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnSignPlayerDistanceStatusChange != null && other.gameObject.CompareTag("Player"))
        {
            Debug.LogFormat("Player exit sign {0} highlight zone.", gameObject.name);
            OnSignPlayerDistanceStatusChange(gameObject, false);
        }
    }
}
