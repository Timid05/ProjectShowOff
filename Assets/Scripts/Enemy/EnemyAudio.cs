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
    AudioClip decoySpawnClip;
    [SerializeField]
    AudioClip deathClip;

    private void OnEnable()
    {
        PlayerActions.OnPlayerHit += PlayRandomJumpscare;
        EnemiesInfo.OnEnragedAttacks += PlayDecoyClip;
        EnemiesInfo.OnEnemyObjectRemoved += PlayDeath;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerHit -= PlayRandomJumpscare;
        EnemiesInfo.OnEnragedAttacks -= PlayDecoyClip;
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

    public void PlayRandomJumpscare()
    {
        if (jumpscareClips.Length > 0)
        {
            source.PlayOneShot(jumpscareClips[Random.Range(0, jumpscareClips.Length)]);
        }
    }

    public void PlayDeath(GameObject removedEnemy)
    {
        if (removedEnemy == gameObject)
        {
            source.PlayOneShot(deathClip);
        }
    }

    public void PlayDecoyClip(GameObject attacker)
    {
        if (attacker == gameObject)
        {
            source.PlayOneShot(decoySpawnClip);
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
