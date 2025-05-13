using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FlashlightActions : MonoBehaviour
{
    new Light light;
    HDAdditionalLightData lightHD;

    float regularIntensity;
    float regularOuterAngle;
    [SerializeField] float flashbangIntensity = 400000f;
    [SerializeField] float flashbangOuterAngle = 45f;
    [SerializeField] float flashbangSpeed = 1f;
    [SerializeField] float flashlightCooldownTime = 15f;
    float flashbangPercentage = 0;
    bool flashbangActive = false;
    bool flashlightCooldownActive = false;

    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();
        lightHD = gameObject.GetComponent<HDAdditionalLightData>();
        light.enabled = false;
        regularOuterAngle = light.spotAngle;
        regularIntensity = lightHD.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashlightCooldownActive)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                light.enabled = !light.enabled;
            }
            else if (light.enabled && Input.GetKeyDown(KeyCode.Mouse1)) { Flashbang(); }

            // Increase flashlight values if the flashbang is active and the values haven't reached the max yet.
            if (flashbangActive)
            {
                if (lightHD.intensity != flashbangIntensity && light.spotAngle != flashbangOuterAngle) { FlashbangAnimation(); }
                else
                {
                    // Once flashbang animation has finished (AKA values have reached frashbang levels), disable flashlight until cooldown is over.
                    light.enabled = false;
                    Flashbang();
                    flashlightCooldownActive = true;
                    StartCoroutine(FlashlightCooldown());
                }
            }
        }
    }

    IEnumerator FlashlightCooldown()
    {

        Debug.Log("Flashlight cooldown active.");
        yield return new WaitForSeconds(flashlightCooldownTime);
        flashlightCooldownActive = false;
        Debug.Log("Flashlight cooldown ended.");

    }

    void Flashbang()
    {
        flashbangActive = !flashbangActive;
        // Change flashlight mode depending on what is currently set.
        if (lightHD.intensity != regularIntensity && light.spotAngle != regularOuterAngle)
        {
            lightHD.SetIntensity(regularIntensity);
            light.spotAngle = regularOuterAngle;

            // Reduce flashbang percentage back to zero when switching back to the regular mode. This is so that the flashbang "animation" plays again when activating flashbang mode the next time.
            flashbangPercentage = 0;
        }
        Debug.LogFormat("Set Light angle to {0} and intensity to {1}.", light.spotAngle, lightHD.intensity);
    }

    void FlashbangAnimation()
    {
        //Play flashbang "animation" by increasing light values until they reach the flashbang amounts.
        //Debug.Log("Playing flashbang animation.");

        lightHD.intensity = Mathf.Lerp(lightHD.intensity, flashbangIntensity, flashbangPercentage);
        light.spotAngle = Mathf.Lerp(light.spotAngle, flashbangOuterAngle, flashbangPercentage);

        flashbangPercentage += flashbangSpeed * Time.deltaTime;
    }
}
