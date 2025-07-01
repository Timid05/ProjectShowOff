using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundTrigger : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    bool canTrigger = false;
    [SerializeField]
    float triggerRadius;

    private void OnEnable()
    {
        GameStateActions.OnChoiceMade += ChoiceMade;
        PlayerActions.OnPlayerRespawn += RespawnTrigger;
    }

    private void OnDisable()
    {
        GameStateActions.OnChoiceMade -= ChoiceMade;
        PlayerActions.OnPlayerRespawn -= RespawnTrigger;
    }

    private void Awake()
    {   
        canTrigger = false;
    }

    private void Start()
    {
        GuidancePointsActions.OnHellhoundSpawned?.Invoke(transform.position);
        HideHound();
    }

    public void ToggleTrigger(bool enable)
    {
        canTrigger = enable;
    }

    public float GetDistanceFromPlayer()
    {
        return Mathf.Abs(Vector3.Magnitude(transform.position - player.position));
    }

    void RespawnTrigger()
    {
        canTrigger = true;
    }

    void ChoiceMade(bool choice)
    {
        ShowHound();
        canTrigger = true;
    }

    void HideHound()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void ShowHound()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }


    private void Update()
    {
        if (canTrigger)
        {
            if (GetDistanceFromPlayer() < triggerRadius)
            {
                HellhoundActions.OnHellhoundFightTriggered?.Invoke();
                EnemiesInfo.RemoveAllEnemies();
                canTrigger = false;
            }
        }
    }

}
