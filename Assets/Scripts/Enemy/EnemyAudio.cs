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
    AudioClip deathClip;

    private void OnEnable()
    {
        PlayerActions.OnPlayerHit += PlayRandomJumpscareClip;
        EnemiesInfo.OnEnragedAttacks += PlayRandomDecoyClip;
        EnemiesInfo.OnEnemyObjectRemoved += PlayDeath;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerHit -= PlayRandomJumpscareClip;
        EnemiesInfo.OnEnragedAttacks -= PlayRandomDecoyClip;
        EnemiesInfo.OnEnemyObjectRemoved -= PlayDeath;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        PlayRandomSpawnClip();
    }
    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayRandomJumpscareClip()
    {
        if (jumpscareClips.Length > 0)
        {
            source.PlayOneShot(jumpscareClips[Random.Range(0, spawnClips.Length)]);
        }
    }

    public void PlayDeath(GameObject removedEnemy)
    {
        if (removedEnemy == gameObject)
        {
            source.PlayOneShot(deathClip);
        }
    }

    public void PlayRandomDecoyClip(GameObject attacker)
    {
        if (attacker == gameObject)
        {
            if (decoySpawnClips.Length > 0)
            {
                source.PlayOneShot(decoySpawnClips[Random.Range(0, spawnClips.Length)]);
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
