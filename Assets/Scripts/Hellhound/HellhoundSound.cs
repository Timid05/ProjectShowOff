using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundSound : MonoBehaviour
{
    AudioSource source;
    [SerializeField]
    AudioClip[] howlClips;
    [SerializeField]
    AudioClip[] growlClips;
    [SerializeField]
    AudioClip[] attackClips;
    [SerializeField]
    AudioClip flashedClip;
    [SerializeField]
    AudioClip deathClip;

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundFightTriggered += PlayRandomHowl;
        HellhoundActions.OnCharge += PlayRandomHowl;
        HellhoundActions.OnGrowlTriggered += PlayGrowl;
        HellhoundActions.OnPounce += PlayRandomAttack;
        HellhoundActions.OnHellhoundFlashed += PlayFlashed;
        HellhoundActions.OnHellhoundDeath += PlayDeath;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundFightTriggered -= PlayRandomHowl;
        HellhoundActions.OnCharge -= PlayRandomHowl;
        HellhoundActions.OnGrowlTriggered -= PlayGrowl;
        HellhoundActions.OnPounce -= PlayRandomAttack;
        HellhoundActions.OnHellhoundFlashed -= PlayFlashed;
        HellhoundActions.OnHellhoundDeath -= PlayDeath;
    }

    private void Awake()
    {
        source = transform.GetComponentInChildren<AudioSource>();
    }

    void PlayRandomHowl()
    {
        if (howlClips.Length > 0)
        {
            int randomIndex = Random.Range(0, howlClips.Length);
            source.PlayOneShot(howlClips[randomIndex]);
        }
    }

    void PlayGrowl()
    {
        if (growlClips.Length > 0)
        {
            int randomIndex = Random.Range(0, growlClips.Length);
            source.PlayOneShot(growlClips[randomIndex]);
        }
    }

    void PlayRandomAttack()
    {
        if (attackClips.Length > 0)
        {
            int randomIndex = Random.Range(0, attackClips.Length);
            source.PlayOneShot(attackClips[randomIndex]);
        }
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
