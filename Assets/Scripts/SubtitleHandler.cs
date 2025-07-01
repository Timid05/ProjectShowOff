using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleHandler : MonoBehaviour
{
    public static Action<AudioClip> OnPlayAudioWithSubtitles;
    [SerializeField]
    UDictionary<AudioClip, string> audioSubtitles = new UDictionary<AudioClip, string>();
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    TextMeshProUGUI textField;

    private void OnEnable()
    {
        textField.gameObject.SetActive(false);
        OnPlayAudioWithSubtitles += PlayClipWithSubtitles;
    }

    private void OnDisable()
    {
        OnPlayAudioWithSubtitles -= PlayClipWithSubtitles;
    }

    void PlayClipWithSubtitles(AudioClip clip)
    {
        if (audioSubtitles.Keys.Contains(clip) && audioSource != null && textField != null)
        {
            //Debug.Log("Playing clip " + clip.name);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            textField.text = audioSubtitles[clip];
            textField.gameObject.SetActive(true);
            audioSource.PlayOneShot(clip);
            StartCoroutine(WaitForClipEnd(clip.length));
        }
        else
        {
            Debug.LogWarning("Could not play clip with subtitles, not all variables were assigned");
        }
    }

    IEnumerator WaitForClipEnd(float time) 
    {
        yield return new WaitForSeconds(time);
        textField.gameObject.SetActive(false);
    }
}
