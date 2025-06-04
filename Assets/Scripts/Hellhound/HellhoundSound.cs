using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundSound : MonoBehaviour
{
    AudioSource source;
    [SerializeField]
    AudioClip howlClip;
    [SerializeField]
    AudioClip[] growlClips;
    [SerializeField]
    AudioClip pounceClip;
    [SerializeField]
    AudioClip flashedClip;
    [SerializeField]
    AudioClip deathClip;

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundFightTriggered += PlayHowl;
        HellhoundActions.OnCharge += PlayHowl;
        HellhoundActions.OnGrowlTriggered += PlayGrowl;
        HellhoundActions.OnPounce += PlayPounce;
        HellhoundActions.OnHellhoundFlashed += PlayFlashed;
        HellhoundActions.OnHellhoundDeath += PlayDeath;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundFightTriggered -= PlayHowl;
        HellhoundActions.OnCharge -= PlayHowl;
        HellhoundActions.OnGrowlTriggered -= PlayGrowl;
        HellhoundActions.OnPounce -= PlayPounce;
        HellhoundActions.OnHellhoundFlashed -= PlayFlashed;
        HellhoundActions.OnHellhoundDeath -= PlayDeath;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayHowl()
    {
        source.PlayOneShot(howlClip);
    }

    void PlayGrowl()
    {
        if (growlClips.Length > 0)
        {
            int randomIndex = Random.Range(0, growlClips.Length);
            source.PlayOneShot(growlClips[randomIndex]);
        }
    }

    void PlayPounce()
    {
        source.PlayOneShot(pounceClip);
    }

    void PlayFlashed()
    {
        source.PlayOneShot(flashedClip);
    }

    void PlayDeath()
    {
        source.PlayOneShot(deathClip);
    }
}
