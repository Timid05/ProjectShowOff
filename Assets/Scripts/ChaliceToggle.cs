using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaliceToggle : MonoBehaviour
{
    [SerializeField]
    GameObject chalice;

    private void OnEnable()
    {
        GameStateActions.OnFirstDomeVisit += ShowChalice;
        GameStateActions.OnChaliceReturned+= HideChalice;
    }

    private void OnDisable()
    {
        GameStateActions.OnFirstDomeVisit -= ShowChalice;
        GameStateActions.OnChaliceReturned -= HideChalice;
    }

    void Start()
    {
        if (chalice != null)
        {
            chalice.SetActive(false);
        }
    }

    void ShowChalice()
    {
        if (chalice != null)
        {
            chalice.SetActive(true);
        }
    }

    void HideChalice()
    {
        if (chalice != null)
        {
            chalice.SetActive(false);
        }
    }
}
