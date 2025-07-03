using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    GameObject deathMessage;
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
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDead -= DisplayDeath;
        PlayerActions.OnHealthUpdated -= UpdateHealth;
        GameStateActions.OnGamePause -= GamePaused;
        PlayerActions.OnPlayerHitBy -= HellhoundAttack;
    }

    private void Awake()
    {
        deathMessage.SetActive(false);
        hellhoundFX.SetActive(false);
    }

    public void RestartClicked()
    {
        if (!HellhoundActions.hellhoundFightOngoing)
        {
            GameStateActions.OnRespawnEnemies?.Invoke();
        }     
        PlayerActions.OnPlayerRespawn?.Invoke();
        GameStateActions.playerDead = false;
        EnemiesInfo.EnableCachedEnemies();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathMessage.SetActive(false);
        health.enabled = true;
    }

    public void BeginFromStartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameStateActions.Reset();
        HellhoundActions.hellhoundFightOngoing = false;
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
        EnemiesInfo.RemoveAllEnemies();
        GameStateActions.playerDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathMessage.SetActive(true);
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
