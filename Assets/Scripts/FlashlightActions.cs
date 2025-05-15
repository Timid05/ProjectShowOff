using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightActions : MonoBehaviour
{
    new Light light;
    float regularIntensity;
    float regularOuterAngle;
    [SerializeField] float flashbangIntensity = 400000f;
    [SerializeField] float flashbangOuterAngle = 45f;
    bool flashbangActive = false;

    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();
        light.enabled = false;
        regularOuterAngle = light.spotAngle;
        regularIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            light.enabled = !light.enabled;
        }
        else if(light.enabled && Input.GetKeyDown(KeyCode.Mouse1)) { Flashbang(); }
    }

    void Flashbang()
    {
        flashbangActive = !flashbangActive;
        // Change flashlight mode depending on what is currently set.
        if(light.intensity != flashbangIntensity && light.spotAngle != flashbangOuterAngle)
        {
            light.intensity = flashbangIntensity;
            light.spotAngle = flashbangOuterAngle;
        }
        else
        {
            light.intensity = regularIntensity;
            light.spotAngle = regularOuterAngle;
        }
    }
}
