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


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandomVoiceLineRoutine());
    }

    private IEnumerator PlayRandomVoiceLineRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            PlayRandomVoiceLine();
        }
    }

    void PlayRandomVoiceLine()
    {
        if (audioClips.Length == 0) return;

        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        SubtitleHandler.OnPlayAudioWithSubtitles?.Invoke(clip);

        //audioSource.clip = clip;
        //audioSource.Play();
        //Debug.Log("Random Voiceline has played");
    }
}
