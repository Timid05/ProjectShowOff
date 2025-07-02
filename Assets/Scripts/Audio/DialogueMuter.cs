using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueMuter : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public AudioSource[] audioSourcesToFade;
    public float fadeDuration = 1f;

    private bool wasDialogueRunning = false;
    private Coroutine fadeCoroutine;
    private PlayerMovement playerMovement;

    void Start()
    {
        if (dialogueRunner == null)
        {
            Debug.LogWarning("DialogueRunner not assigned!");
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogWarning("PlayerMovement script not found in scene.");
        }
    }

    void Update()
    {
        if (dialogueRunner == null)
            return;

        bool isRunning = dialogueRunner.IsDialogueRunning;

        if (isRunning != wasDialogueRunning)
        {
            wasDialogueRunning = isRunning;

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeAudio(isRunning));
        }
    }

    IEnumerator FadeAudio(bool fadeOut)
    {
        if (playerMovement != null)
            playerMovement.overrideStaminaAudio = fadeOut;

        float timer = 0f;

        float[] originalVolumes = new float[audioSourcesToFade.Length];
        for (int i = 0; i < audioSourcesToFade.Length; i++)
        {
            if (audioSourcesToFade[i] != null)
                originalVolumes[i] = audioSourcesToFade[i].volume;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            for (int i = 0; i < audioSourcesToFade.Length; i++)
            {
                if (audioSourcesToFade[i] == null) continue;

                float targetVolume = fadeOut ? 0f : originalVolumes[i];
                float startVolume = fadeOut ? originalVolumes[i] : 0f;

                audioSourcesToFade[i].volume = Mathf.Lerp(startVolume, targetVolume, t);
            }

            yield return null;
        }

        // Final volume clamp
        for (int i = 0; i < audioSourcesToFade.Length; i++)
        {
            if (audioSourcesToFade[i] != null)
                audioSourcesToFade[i].volume = fadeOut ? 0f : originalVolumes[i];
        }

        Debug.Log($"Audio fade {(fadeOut ? "out" : "in")} completed.");
    }
}
