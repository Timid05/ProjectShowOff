using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private int currentHealth = 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        PlayerActions.OnHealthUpdated?.Invoke(maxHealth, currentHealth);
    }

    private void OnEnable()
    {
        PlayerActions.OnPlayerDamaged += TakeDamage;
        PlayerActions.OnPlayerRespawn += Respawn;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDamaged -= TakeDamage;
        PlayerActions.OnPlayerRespawn -= Respawn;
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        PlayerActions.OnHealthUpdated?.Invoke(maxHealth, currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerActions.OnHealthUpdated?.Invoke(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            PlayerActions.OnPlayerDead?.Invoke();           
        }
    }
}
