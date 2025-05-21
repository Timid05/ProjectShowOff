using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    AudioSource source;

    [SerializeField]
    AudioClip[] spawnClips;

    [SerializeField]
    AudioClip jumpscareClip;
    

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

    public void PlayJumpscare()
    {
        source.PlayOneShot(jumpscareClip);
    }

    public void PlayRandomSpawnClip()
    {
        if (spawnClips.Length > 0)
        {
            source.PlayOneShot(spawnClips[Random.Range(0, spawnClips.Length)]);
        }
    }
}
