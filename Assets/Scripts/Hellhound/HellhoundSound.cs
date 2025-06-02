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

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundFightTriggered += PlayHowl;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundFightTriggered -= PlayHowl;
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
}
