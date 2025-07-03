using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSignPrompt : MonoBehaviour
{
    GameObject prompt;

    private void OnEnable()
    {
        GameStateActions.OnFirstSignSpotted += ShowPrompt;
    }

    private void OnDisable()
    {
        GameStateActions.OnFirstSignSpotted -= ShowPrompt;
    }

    private void Awake()
    {
        prompt = transform.GetChild(0).gameObject;
        prompt.SetActive(false);
    }

    void ShowPrompt(bool toggle)
    {
        prompt?.SetActive(toggle);
    }

    void HidePrompt()
    {
        prompt?.SetActive(false);
    }

    private void Update()
    {
        if (prompt != null && prompt.activeSelf && GameStateActions.firstSignSpotted)
        {
            HidePrompt();
        }

        if (MapVisibility.mapOpen && !GameStateActions.firstSignSpotted && prompt.activeSelf)
        {
            GameStateActions.firstSignSpotted = true;
        }

        if (GameStateActions.playerDead && prompt.activeSelf)
        {
            HidePrompt();
        }
    }
}
