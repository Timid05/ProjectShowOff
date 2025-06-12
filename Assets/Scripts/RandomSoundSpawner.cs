using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundSpawner : MonoBehaviour
{
    public AudioClip[] soundClips; 
    public float radius = 10f; 
    public float minDelay = 2f;
    public float maxDelay = 5f;
    private AudioSource source;

    public GameObject player;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandomSoundRoutine());
    }

    private IEnumerator PlayRandomSoundRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            PlayRandomSound();
        }
    }

    void PlayRandomSound()
    {
        if (soundClips.Length == 0 || player == null) return;

        AudioClip clip = soundClips[Random.Range(0, soundClips.Length)];

        Vector3 randomPos = player.transform.position + Random.insideUnitSphere * radius;
        randomPos.y = player.transform.position.y;

        transform.position = randomPos;

        source.clip = clip;
        source.maxDistance = radius;
        source.Play();
    }
}
