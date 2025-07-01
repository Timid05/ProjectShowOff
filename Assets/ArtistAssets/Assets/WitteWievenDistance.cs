using UnityEngine;

public class ClosestObjectDistance : MonoBehaviour
{
    [Tooltip("Tag of objects to find")]
    public string targetTag = "Target";

    [Tooltip("Distance at which the intensity is 0")]
    public float maxDistance = 10f;

    [Tooltip("Current intensity from 0 to 1 based on closest object's distance")]
    [Range(0f, 1f)]
    public float intensity = 0f;

    void Update()
    {
        GameObject closestObj = FindClosestWithTag(targetTag);
        if (closestObj != null)
        {
            float distance = Vector3.Distance(transform.position, closestObj.transform.position);
            // Normalize intensity: closer distance means higher intensity
            intensity = Mathf.Clamp01(1 - (distance / maxDistance));
        }
        else
        {
            intensity = 0f;
        }

        // Use intensity to trigger your custom pass, e.g.
        // CustomPassController.SetIntensity(intensity);
        //Debug.Log($"Intensity based on closest {targetTag}: {intensity * 100f}%");
    }

    GameObject FindClosestWithTag(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject obj in taggedObjects)
        {
            float dist = Vector3.Distance(obj.transform.position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }
        return closest;
    }
}
