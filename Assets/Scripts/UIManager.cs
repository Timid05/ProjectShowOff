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
    [SerializeField]
    TextMeshProUGUI mapPrompt;
    [SerializeField]
    GameObject hellhoundFX;

    Animator animator;

    private void OnEnable()
    {
        if (hellhoundFX != null)
        {
            animator = hellhoundFX.GetComponent<Animator>();
            PlayerActions.OnPlayerHitBy += HellhoundAttack;
        }
        PlayerActions.OnPlayerDead += DisplayDeath;
        PlayerActions.OnHealthUpdated += UpdateHealth;
        GameStateActions.OnGamePause += GamePaused;
        GameStateActions.OnFirstMapOpen += DisableMapPrompt;      
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDead -= DisplayDeath;
        PlayerActions.OnHealthUpdated -= UpdateHealth;
        GameStateActions.OnGamePause -= GamePaused;
        GameStateActions.OnFirstMapOpen -= DisableMapPrompt;
        PlayerActions.OnPlayerHitBy += HellhoundAttack;
    }

    private void Awake()
    {
        deathMessage.enabled = false;
        hellhoundFX.SetActive(false);
    }

    void HellhoundAttack(GameObject attacker)
    {
        if (attacker.CompareTag("Hellhound"))
        {
            hellhoundFX.SetActive(true);
            Debug.Log("triggering fx");
            animator.SetTrigger("Attacked");
            StartCoroutine("WaitforFX");
        }
    }

    IEnumerator WaitforFX()
    {
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        hellhoundFX.SetActive(false);
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

    void DisableMapPrompt()
    {
        if (mapPrompt != null)
        {
            mapPrompt.enabled = false;
        }
    }
}
