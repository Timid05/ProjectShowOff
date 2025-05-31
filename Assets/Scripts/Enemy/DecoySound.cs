using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoySound : MonoBehaviour
{
    AudioSource source;
    EnemyController parentController;

    [SerializeField]
    AudioClip decoyHitClip, decoyDisappearClip;

    private void OnEnable()
    {
        EnemiesInfo.OnDecoyHit += PlayHit;
        EnemiesInfo.OnDecoyDestroyed += PlayDisappear;
    }

    private void OnDisable()
    {
        EnemiesInfo.OnDecoyHit -= PlayHit;
        EnemiesInfo.OnDecoyDestroyed -= PlayDisappear;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        parentController = transform.parent.GetComponent<EnemyController>();
    }

    void PlayHit(GameObject destroyed)
    {
        if (destroyed == gameObject)
        {
            source.PlayOneShot(decoyHitClip);
        }
    }

    void PlayDisappear(GameObject destroyed)
    {
        if (destroyed == gameObject)
        {
            source.PlayOneShot(decoyDisappearClip);
        }
    }
}
