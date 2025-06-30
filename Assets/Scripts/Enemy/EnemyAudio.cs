using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    AudioSource source;

    [SerializeField]
    AudioClip[] spawnClips;

    [SerializeField]
    AudioClip[] jumpscareClips;
    [SerializeField]
    AudioClip[] decoySpawnClips;
    [SerializeField]
    AudioClip[] deathClips;

    private void OnEnable()
    {
        PlayerActions.OnPlayerHit += PlayRandomJumpscareClip;
        EnemiesInfo.OnEnragedAttacks += PlayRandomDecoyClip;
        EnemiesInfo.OnEnemyObjectRemoved += PlayRandomDeath;

        PlayRandomSpawnClip();
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerHit -= PlayRandomJumpscareClip;
        EnemiesInfo.OnEnragedAttacks -= PlayRandomDecoyClip;
        EnemiesInfo.OnEnemyObjectRemoved -= PlayRandomDeath;
    }

    private void Awake()
    {
        source = transform.parent.gameObject.GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayRandomJumpscareClip()
    {
        if (jumpscareClips.Length > 0)
        {
            source.PlayOneShot(jumpscareClips[Random.Range(0, jumpscareClips.Length)]);
        }
    }

    public void PlayRandomDeath(GameObject removedEnemy)
    {
        if (removedEnemy == gameObject)
        {
            source.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)]);
        }
    }

    public void PlayRandomDecoyClip(GameObject attacker)
    {
        if (attacker == gameObject)
        {
            if (decoySpawnClips.Length > 0)
            {
                source.PlayOneShot(decoySpawnClips[Random.Range(0, decoySpawnClips.Length)]);
            }
        }
    }

    public void PlayRandomSpawnClip()
    {
        if (spawnClips.Length > 0)
        {
            source.PlayOneShot(spawnClips[Random.Range(0, spawnClips.Length)]);
        }
    }
}
