using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System;

public class FlashlightActions : MonoBehaviour
{
    new Light light;
    HDAdditionalLightData lightHD;
    SphereCollider sphereCollider;
    List<GameObject> witteWievenInFlashRange;

    public static event Action<bool> OnFlashlightStatusChange;

    float regularIntensity;
    float regularOuterAngle;

    [SerializeField] float tamfanaIntensity = 600000f;
    [SerializeField] float tamfanaOuterAngle = 135f;
    [SerializeField] float tamfanaColor = 3800f;

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
        PlayerInteraction.OnCharacterTalk += FlashlightAvailability;
        GameManager.OnAcceptTanfanaChoice += HolyFlashlight;

        light = gameObject.GetComponent<Light>();
        lightHD = gameObject.GetComponent<HDAdditionalLightData>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();

        witteWievenInFlashRange = new List<GameObject>();

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
                ChangeFlashlightStatus();
            }

            //Debug option. REMOVE later.
            if(Input.GetKeyDown(KeyCode.T)) { HolyFlashlight(); }

            else if (Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                light.enabled = true;
                Flashbang(); 
            }

            // Increase flashlight values if the flashbang is active and the values haven't reached the max yet.
            if (flashbangActive)
            {
                if (lightHD.intensity != flashbangIntensity && light.spotAngle != flashbangOuterAngle) { FlashbangAnimation(); }
                else
                {
                    // Once flashbang animation has finished (AKA values have reached frashbang levels), disable flashlight until cooldown is over and remove all Witte Wieven in range.
                    ChangeFlashlightStatus();
                    FlashlightDispell();
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

    // Disable flashlight, while character is busy with something else, like talking to a character.
    void FlashlightAvailability(bool characterBusy)
    {
        if(characterBusy) { flashlightCooldownActive = true; }
        else { flashlightCooldownActive = false; }
    }

    void ChangeFlashlightStatus()
    {
        light.enabled = !light.enabled;
        // Send out delegate so that Witte Wieven can change visibility as well.
        if(OnFlashlightStatusChange != null) { OnFlashlightStatusChange(light.enabled); }

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

        lightHD.intensity = Mathf.Lerp(lightHD.intensity, flashbangIntensity, flashbangPercentage);
        light.spotAngle = Mathf.Lerp(light.spotAngle, flashbangOuterAngle, flashbangPercentage);

        flashbangPercentage += flashbangSpeed * Time.deltaTime;
    }

    void FlashlightDispell()
    {
        //Remove all Witte Wieven that are in range of the flashbang.
        foreach(GameObject witteWief in witteWievenInFlashRange)
        {
            Destroy(witteWief);
        }
        witteWievenInFlashRange.Clear();
    }

    void HolyFlashlight()
    {
        // Replace normal flashbang values with the higher tanfana ones.
        flashbangIntensity = tamfanaIntensity;
        flashbangOuterAngle = tamfanaOuterAngle;
        light.colorTemperature = tamfanaColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("WitteWieven"))
        {
            //Debug.LogFormat("Witte wief {0} in flash range.", other.gameObject.name);
            witteWievenInFlashRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WitteWieven"))
        {
            //Debug.LogFormat("Witte wief {0} out of flash range.", other.gameObject.name);
            witteWievenInFlashRange.Remove(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerInteraction.OnCharacterTalk -= FlashlightAvailability;
        GameManager.OnAcceptTanfanaChoice -= HolyFlashlight;
    }
}
