using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanfanaChalice : MonoBehaviour
{
    [SerializeField]
    GameObject chalice;

    private void OnEnable()
    {
        if (chalice != null)
        {
            chalice.SetActive(false);
        }
        GameStateActions.OnChaliceReturned += ShowChalice;
    }

    private void OnDisable()
    {
        GameStateActions.OnChaliceReturned -= ShowChalice;
    }

    void ShowChalice()
    {
        if (chalice != null)
        {
            chalice.SetActive(true);
        }
    }
}
