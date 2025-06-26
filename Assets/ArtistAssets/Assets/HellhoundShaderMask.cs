using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RuntimeShaderPropertyEditor : MonoBehaviour
{
    [Header("Shader Material")]
    public Material fullscreenMaterial;

    [Header("Tracked Object")]
    public Transform trackedObject;

    [Header("Fallback Settings")]
    public Vector2 fallbackMaskPos = new Vector2(0.5f, 0.5f);
    public float maskRadius = 0.3f;
    public float maskSoftness = 0.2f;

    private Camera mainCamera;

    void Start()
    {
        if (fullscreenMaterial == null)
        {
            Debug.LogError("Fullscreen Material is not assigned.");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
        UpdateShaderProperties();
    }

    void Update()
    {
        UpdateShaderProperties();
    }

    void UpdateShaderProperties()
    {
        Vector2 screenUV = fallbackMaskPos;

        if (trackedObject != null && mainCamera != null)
        {
            Vector3 screenPos = mainCamera.WorldToViewportPoint(trackedObject.position);
            screenUV = new Vector2(screenPos.x, screenPos.y);
        }

        fullscreenMaterial.SetVector("_MaskPos", screenUV);
        fullscreenMaterial.SetFloat("_MaskRadius", maskRadius);
        fullscreenMaterial.SetFloat("_MaskSoftness", maskSoftness);
    }
}
