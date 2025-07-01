using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerRandomVoicelines : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    bool disabled = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandomVoiceLineRoutine());
    }

    private void OnEnable()
    {
        PlayerActions.OnDisableRandomPlayerSound += Timer;
    }

    private void OnDisable()
    {
        PlayerActions.OnDisableRandomPlayerSound -= Timer;
    }

    private IEnumerator PlayRandomVoiceLineRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (!GameStateActions.inDialogue && !HellhoundActions.hellhoundFightOngoing && !GameStateActions.playerDead && !disabled)
            {
                PlayRandomVoiceLine();
            }
        }
    }

    void Timer(float delay)
    {
        StartCoroutine(DisableTimer(delay));
    }

    IEnumerator DisableTimer(float waitTime) 
    {
        disabled = true;
        yield return new WaitForSeconds(waitTime);
        disabled = false;
    }

    void PlayRandomVoiceLine()
    {
        if (audioClips.Length == 0) return;

        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
        //Debug.Log("Random Voiceline has played");
    }
}
