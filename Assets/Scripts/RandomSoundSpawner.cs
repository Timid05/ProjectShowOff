using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundSpawner : MonoBehaviour
{
    public AudioClip[] soundClips; 
    public float radius = 10f; 
    public float minDelay = 2f;
    public float maxDelay = 5f;

    public GameObject player;

    private void Start()
    {
        StartCoroutine(PlayRandomSoundRoutine());
    }

    private System.Collections.IEnumerator PlayRandomSoundRoutine()
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

        GameObject soundObject = new GameObject("TempAudio");
        soundObject.transform.position = randomPos;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = radius;
        audioSource.Play();

        Destroy(soundObject, clip.length + 0.5f);
    }
}
