using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPath : MonoBehaviour
{
    [SerializeField] UDictionary<GameObject, GameObject> pathMapConversion;

    private void Awake()
    {
        PathGenerator.OnPathObjectEnabled += AddMapPath;
        PathGenerator.OnPathFail += ResetMapPath;
    }

    void AddMapPath(GameObject pathObject)
    {
        // Makes the map version of every main path object visible.
        pathMapConversion.TryGetValue(pathObject, out GameObject mapPathObject);
        if (mapPathObject != null) { mapPathObject.SetActive(true); }
    }

    void ResetMapPath()
    {
        // If path generation fails we reset all map paths to not be visible.
        foreach(GameObject mapPath in pathMapConversion.Values) { mapPath.SetActive(false); }
    }

    private void OnDestroy()
    {
        PathGenerator.OnPathObjectEnabled -= AddMapPath;
        PathGenerator.OnPathFail -= ResetMapPath;
    }

}
