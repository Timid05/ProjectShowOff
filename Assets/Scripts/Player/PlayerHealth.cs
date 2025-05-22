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
        PlayerActions.OnHealthUpdated(maxHealth, currentHealth);
    }

    private void OnEnable()
    {
        PlayerActions.OnPlayerDamaged += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDamaged -= TakeDamage;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerActions.OnHealthUpdated(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            PlayerActions.OnPlayerDead?.Invoke();
            Debug.Log("You ded homie");
        }
    }
}
