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

    public void ToggleTrigger(bool enable)
    {
        canTrigger = enable;
    }

    public float GetDistanceFromPlayer()
    {
        return Mathf.Abs(Vector3.Magnitude(transform.position - player.position));
    }

    private void Update()
    {
        if (canTrigger)
        {
            if (GetDistanceFromPlayer() < triggerRadius)
            {
                HellhoundActions.OnHellhoundFightTriggered?.Invoke();
                canTrigger = false;
            }
        }
    }

}
