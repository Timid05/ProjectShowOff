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
    private AudioClip[] audioClipsHH;

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
            audioSource.PlayOneShot(audioClipsWW[Random.Range(0, audioClipsWW.Length)]); ;
        }
        else if (hitBy.CompareTag("Hellhound"))
        {
            audioSource.PlayOneShot(audioClipsHH[Random.Range(0, audioClipsHH.Length)]);
        }
    }

    void PlayEnemyHint()
    {
        audioSource.PlayOneShot(audioClipsWW[0]);
    }
}
