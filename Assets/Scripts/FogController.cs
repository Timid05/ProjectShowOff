using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FogController : MonoBehaviour
{   
    public LocalVolumetricFog fog;    
    public Transform target;

    Vector3 oldPosition;

    void Start()
    {
        fog.transform.position = target.position;
        oldPosition = target.position;
    }
    
    public void SetFogDistance(float distance)
    {
        fog.parameters.meanFreePath = distance;
    }

    public void EnableFog()
    {
        fog.enabled = true;
    }

    public void DisableFog()
    {
        fog.enabled = false;
    }

    void UpdatePosition()
    {
        if (oldPosition != target.position)
        {
            fog.transform.position = target.position;
            oldPosition = target.position;          
        }
    }

    void Update()
    {
        if (target != null)
        {
            UpdatePosition();
        }
    }
}
