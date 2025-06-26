using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winfred : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hellhoundParts;

    [SerializeField]
    private float hueShiftSpeed = 0.2f;

    [SerializeField]
    private float emissionIntensity = 500f;

    private Material[] materials;

    private float hue;

    private void Start()
    {
        materials = new Material[hellhoundParts.Length];

        for (int i = 0; i < hellhoundParts.Length; i++)
        {
            Renderer rend = hellhoundParts[i].GetComponent<Renderer>();
            if (rend != null)
            {
                materials[i] = rend.material;

                materials[i].EnableKeyword("_EMISSIVE_COLOR_MAP");
                materials[i].SetColor("_EmissiveColor", Color.black); // init
                materials[i].SetFloat("_EmissiveIntensity", 0f);
            }
        }
    }

    void Update()
    {
        hue += hueShiftSpeed * Time.deltaTime;
        hue %= 1f;

        Color color = Color.HSVToRGB(hue, 1f, 1f);
        Color emissiveColor = color.linear * emissionIntensity;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] != null)
            {
                materials[i].SetColor("_EmissiveColor", emissiveColor * 100f);
                materials[i].SetFloat("_EmissiveIntensity", emissionIntensity);
            }
        }
    }
}
