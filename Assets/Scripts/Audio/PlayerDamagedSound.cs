using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClipsWW;
    [SerializeField]
    private AudioClip[] audioClipsJumpscare;
    [SerializeField]
    private AudioClip[] audioClipsHH;
    [SerializeField]
    private AudioClip audioClipHint;

    private void OnEnable()
    {
        PlayerActions.OnPlayerHitBy += PlayDamageSound;
        GameStateActions.OnFirstEnemyEncounter += PlayEnemyHint;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerHitBy -= PlayDamageSound;
        GameStateActions.OnFirstEnemyEncounter -= PlayEnemyHint;
    }

    private void PlayDamageSound(GameObject hitBy)
    {
        if (hitBy.CompareTag("WitteWieven"))
        {
            //audioSource.PlayOneShot(audioClipsWW[Random.Range(0, audioClipsWW.Length)]);
            audioSource.PlayOneShot(audioClipsJumpscare[Random.Range(0, audioClipsJumpscare.Length)]);
        }
        else if (hitBy.CompareTag("Hellhound"))
        {
            audioSource.PlayOneShot(audioClipsHH[Random.Range(0, audioClipsHH.Length)]);
        }
    }

    void PlayEnemyHint()
    {
        SubtitleHandler.OnPlayAudioWithSubtitles?.Invoke(audioClipHint);
        PlayerActions.OnDisableRandomPlayerSound?.Invoke(audioClipHint.length + 2f);
    }
}
