using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShareMainPath : MonoBehaviour
{
    public static event Action<List<GameObject>, GameObject> OnMainPathOnly;
    List<GameObject> mainPathObjectsInArea;

    private void Awake()
    {
        PathGenerator.OnPathGenerated += RemoveNonMainPathObjects;
    }

    private void Start()
    {
        mainPathObjectsInArea = new List<GameObject>();
    }

    // Reduce list of path objects in the area to only the ones on the main path, once generation is done.
    private void RemoveNonMainPathObjects()
    {
        Debug.LogFormat("Area {0} has {1} path objects.", gameObject.name, mainPathObjectsInArea.Count);
        for (int i = mainPathObjectsInArea.Count - 1; i >= 0; i--)
        {
            if(!mainPathObjectsInArea[i].activeSelf) { mainPathObjectsInArea.RemoveAt(i); }
            else { Debug.LogFormat("Area {0} path object {1} is part of the main trail.", gameObject.name, mainPathObjectsInArea[i].name); }
        }
        Debug.LogFormat("Area {0} has {1} main path objects.", gameObject.name, mainPathObjectsInArea.Count);

        // Share main path objects with building spawner and the area it originates from.
        if(OnMainPathOnly != null) { OnMainPathOnly(mainPathObjectsInArea, gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add all path objects in the area to the list.
        if (other.CompareTag("Path"))
        {
            //Debug.LogFormat("Area {0} has path object {1}", gameObject.name, other.gameObject.name);
            GameObject path = other.gameObject;

            if (path != null) { mainPathObjectsInArea.Add(path); }
        }
    }

    private void OnDestroy()
    {
        PathGenerator.OnPathGenerated -= RemoveNonMainPathObjects;
    }
}
