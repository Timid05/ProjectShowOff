using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    TextMeshProUGUI deathMessage;

    private void OnEnable()
    {
        PlayerActions.OnPlayerDead += DisplayDeath;
        PlayerActions.OnHealthUpdated += UpdateHealth;
        GameStateActions.OnGamePause += GamePaused;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDead -= DisplayDeath;
        PlayerActions.OnHealthUpdated -= UpdateHealth;
        GameStateActions.OnGamePause -= GamePaused;
    }

    private void Awake()
    {
        deathMessage.enabled = false;
    }

    void DisplayDeath()
    {
        deathMessage.enabled = true;
        health.enabled = false;
    }

    void UpdateHealth(int maxHealth, int currentHealth)
    {
        health.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    void GamePaused(bool paused)
    {
        if (paused) { health.enabled = false; }
        if (!paused) { health.enabled = true; }
    }
}
