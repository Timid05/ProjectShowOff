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
    [SerializeField] float decoyHitDistance = 20;
    float flashbangPercentage = 0;
    bool flashbangActive = false;
    bool flashlightCooldownActive = false;
    bool hellhoundFlash = false;
    bool houndInFlashRange = false;

    //Flash Indicator
    private float cooldownCoroutineTimer = 0f;

    //audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] flashlightSound;
    [SerializeField] private AudioClip[] flashbangSound;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInteraction.OnCharacterTalk += FlashlightAvailability;
        GameManager.OnAcceptTanfanaChoice += HolyFlashlight;
        PlayerActions.OnPlayerDead += DisableFlashlight;
        EnemiesInfo.OnEnemyObjectRemoved += CheckFlashRange;
        HellhoundActions.OnHellhoundFlashable += HellhoundFlashStatus;

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
                //Play Sound
                audioSource.PlayOneShot(flashlightSound[UnityEngine.Random.Range(0, flashlightSound.Length)]);

                ChangeFlashlightStatus();
            }

            //Debug option. REMOVE later.
            if(Input.GetKeyDown(KeyCode.T)) { HolyFlashlight(); }

            else if (Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                light.enabled = true;

                //Play Sound
                audioSource.PlayOneShot(flashbangSound[UnityEngine.Random.Range(0, flashbangSound.Length)]);

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

            if (light.enabled)
            {
                CastFlashlightRay();
            }
        }
    }

    void DisableFlashlight()
    {
        light.enabled = false;
        this.enabled = false;
    }
    void CastFlashlightRay()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.TryGetComponent<WitteWievenDecoy>(out WitteWievenDecoy decoy) && hit.distance < decoyHitDistance)
            {
                Destroy(decoy.gameObject);
            }
        }
    }


    IEnumerator FlashlightCooldown()
    {
        Debug.Log("Flashlight cooldown active.");
        //Commenting for indicator setup
        //yield return new WaitForSeconds(flashlightCooldownTime);

        //Flash Indicator
        cooldownCoroutineTimer = flashlightCooldownTime;
        while (cooldownCoroutineTimer > 0f)
        {
            cooldownCoroutineTimer -= Time.deltaTime;
            yield return null;
        }
        cooldownCoroutineTimer = 0f;
        flashlightCooldownActive = false;

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
        if (OnFlashlightStatusChange != null) { OnFlashlightStatusChange(light.enabled); }
    }

    void CheckFlashRange(GameObject witteWief)
    {
        if (witteWievenInFlashRange.Contains(witteWief))
        {
            witteWievenInFlashRange.Remove(witteWief);
        }
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

    //Flash Indicator
    public bool IsFlashlightOnCooldown()
    {
        return flashlightCooldownActive;
    }

    public float GetCooldownProgressNormalized()
    {
        if (!flashlightCooldownActive) return 1f; // Ready
        return 1f - Mathf.Clamp01(cooldownCoroutineTimer / flashlightCooldownTime);
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
        if (houndInFlashRange && hellhoundFlash)
        {
            Debug.Log("Flashed hellhound");
            HellhoundActions.OnHellhoundFlashed?.Invoke();
        }

        for (int i = witteWievenInFlashRange.Count -1 ; i >= 0; i--)
        {
            EnemiesInfo.RemoveEnemy(witteWievenInFlashRange[i].GetComponent<EnemyController>().fsm);
        }
        //Remove all Witte Wieven that are in range of the flashbang.
        witteWievenInFlashRange.Clear();
    }

    void HolyFlashlight()
    {
        // Replace normal flashbang values with the higher tanfana ones.
        flashbangIntensity = tamfanaIntensity;
        flashbangOuterAngle = tamfanaOuterAngle;
        light.colorTemperature = tamfanaColor;
    }

    void HellhoundFlashStatus(bool flashable)
    {
        hellhoundFlash = flashable;
        Debug.Log("hellhound flashable: " + flashable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("WitteWieven"))
        {
            //Debug.LogFormat("Witte wief {0} in flash range.", other.gameObject.name);
            witteWievenInFlashRange.Add(other.gameObject);
        }

        if(other.gameObject.CompareTag("Hellhound"))
        {
            Debug.Log("Hellhound in range");
            houndInFlashRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WitteWieven"))
        {
            //Debug.LogFormat("Witte wief {0} out of flash range.", other.gameObject.name);
            witteWievenInFlashRange.Remove(other.gameObject);
        }

        if (other.gameObject.CompareTag("Hellhound"))
        {
            Debug.Log("Hellhound out of range");
            houndInFlashRange = true;
        }
    }

    private void OnDestroy()
    {
        PlayerInteraction.OnCharacterTalk -= FlashlightAvailability;
        GameManager.OnAcceptTanfanaChoice -= HolyFlashlight;
        PlayerActions.OnPlayerDead -= DisableFlashlight;
        EnemiesInfo.OnEnemyObjectRemoved -= CheckFlashRange;
        HellhoundActions.OnHellhoundFlashable -= HellhoundFlashStatus;
    }
}
