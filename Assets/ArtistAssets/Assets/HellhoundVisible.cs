using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DistanceTriggerWithCustomPass : MonoBehaviour
{
    public GameObject player;
    public GameObject targetObject;
    public float triggerDistance = 10f;
    public CustomPassVolume customPassToToggle;
    public AudioSource source;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float clipVolume = 1f;

    private bool isInRange = false;

    private void Awake()
    {
        customPassToToggle.enabled = false;
    }

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
        HellhoundActions.OnHellhoundVisible?.Invoke();
        source.PlayOneShot(clip, clipVolume);
        await Task.Delay(1000);
        customPassToToggle.enabled = false;
    }

    void OnExitRange()
    {
        Debug.Log($"{targetObject.name} is now out of range. Disabling custom pass.");
        
    }
}
