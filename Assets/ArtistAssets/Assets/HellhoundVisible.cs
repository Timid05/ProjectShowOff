using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DistanceTriggerWithCustomPass : MonoBehaviour
{
    public GameObject player;
    public GameObject targetObject;
    public float triggerDistance = 10f;
    public CustomPassVolume customPassToToggle;

    private bool isInRange = false;

    void Update()
    {
        if (player == null || targetObject == null || customPassToToggle == null) return;

        float distance = Vector3.Distance(player.transform.position, targetObject.transform.position);

        if (distance <= triggerDistance && !isInRange)
        {
            isInRange = true;
            OnEnterRange();
        }
        else if (distance > triggerDistance && isInRange)
        {
            isInRange = false;
            OnExitRange();
        }
    }

    async void OnEnterRange()
    {
        Debug.Log($"{targetObject.name} is now within range. Enabling custom pass.");
        customPassToToggle.enabled = true;
        await Task.Delay(10000);
        customPassToToggle.enabled = false;
    }

    void OnExitRange()
    {
        Debug.Log($"{targetObject.name} is now out of range. Disabling custom pass.");
        
    }
}
