using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
    [SerializeField] UDictionary<GameObject, GameObject> pathMapConversion;

    private void Awake()
    {
        PathGenerator.OnPathObjectEnabled += AddMapPath;
    }

    void AddMapPath(GameObject pathObject)
    {
        // Makes the map version of every main path object visible.
        pathMapConversion.TryGetValue(pathObject, out GameObject mapPathObject);
        if (mapPathObject != null) { mapPathObject.SetActive(true); }
    }

    private void OnDestroy()
    {
        PathGenerator.OnPathObjectEnabled -= AddMapPath;
    }

}
