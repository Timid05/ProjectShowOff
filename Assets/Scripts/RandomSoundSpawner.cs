using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundSpawner : MonoBehaviour
{
    public AudioClip[] soundClips; // Array of audio clips to randomly choose from
    public float radius = 10f; // Distance around the player to spawn the sound
    public float minDelay = 2f;
    public float maxDelay = 5f;

    public GameObject player; // Assign your player object in the inspector

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

        // Pick a random sound
        AudioClip clip = soundClips[Random.Range(0, soundClips.Length)];

        // Generate a random point around the player
        Vector3 randomPos = player.transform.position + Random.insideUnitSphere * radius;
        randomPos.y = player.transform.position.y; // Keep it at the player's height

        // Create an AudioSource at that point
        GameObject soundObject = new GameObject("TempAudio");
        soundObject.transform.position = randomPos;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f; // Make it 3D
        audioSource.minDistance = 1f;
        audioSource.maxDistance = radius;
        audioSource.Play();

        Destroy(soundObject, clip.length + 0.5f); // Cleanup
    }
}
