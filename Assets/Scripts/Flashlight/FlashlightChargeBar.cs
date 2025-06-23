using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightChargeBar : MonoBehaviour
{
    public FlashlightActions flashlight; 
    public Transform barVisual; 
    public Renderer barRenderer;

    public Color readyColor = Color.green;
    public Color cooldownColor = Color.red;

    private Material mat;
    private Vector3 fullScale = new Vector3(1, 1, 1);
    private Vector3 emptyScale = new Vector3(1, 0.1f, 1);

    void Start()
    {
        mat = barRenderer.material;
    }

    void Update()
    {
        float progress = flashlight.GetCooldownProgressNormalized();

        // Scale bar
        barVisual.localScale = Vector3.Lerp(emptyScale, fullScale, progress);

        // Color transition
        mat.EnableKeyword("_EMISSIVE_COLOR_MAP");
        Color emissive = Color.Lerp(cooldownColor, readyColor, progress);
        mat.SetColor("_EmissiveColor", emissive * 10f);
        mat.SetFloat("_EmissiveIntensity", 20f);
    }
}
