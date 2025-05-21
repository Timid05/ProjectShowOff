using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI health;

    private void OnEnable()
    {
        PlayerActions.OnHealthUpdated += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerActions.OnHealthUpdated += UpdateHealth;
    }


    void UpdateHealth(int maxHealth, int currentHealth)
    {
        health.text = "Health: " + currentHealth + "/" + maxHealth;
    }
}
