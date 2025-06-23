using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VisualEffects : MonoBehaviour
{

    public Volume globalVolume;
    ChromaticAberration chromAb;
    private float currentChromaticIntensity;
    private float maxChromaticIntensity = 100;
    private float minChromaticIntensity = 0;
    FilmGrain FilmGr;
    private float currentFilmGrainIntensity;
    private float maxFilmGrainIntensity = 100;
    private float minFilmGrainIntensity = 0;
    Vignette Vign;
    private float currentVignetteIntensity;
    private float maxVignetteIntensity = 20;
    private float minVignetteIntensity = 0;
    LensDistortion LensDis;
    private float currentLensDistance;
    private float maxLensDistance = 2;
    private float minLensDistance = 0;



    private void OnEnable()
    {
        if (globalVolume.profile.TryGet(out ChromaticAberration ab))
        {
            chromAb = ab;
        }
        currentChromaticIntensity = chromAb.intensity.value;

        if (globalVolume.profile.TryGet(out FilmGrain gr))
        { 
        FilmGr= gr;
        }
        currentFilmGrainIntensity = FilmGr.intensity.value;

        if(globalVolume.profile.TryGet(out Vignette Vig))
        {
            Vign = Vig;
        }
        currentVignetteIntensity = Vign.intensity.value;

        if (globalVolume.profile.TryGet(out LensDistortion Lens))
        {
            LensDis = Lens;
        }
        currentLensDistance = LensDis.intensity.value;

        HellhoundActions.OnPounce += VisualFX;
        HellhoundAnimations.OnPounceAnimDone += VisualFXEnd;
    }

    
    private void OnDisable()
    {
        HellhoundActions.OnPounce -= VisualFX;
        HellhoundAnimations.OnPounceAnimDone -= VisualFXEnd;
    }

    void VisualFX()
    {
        chromAb.intensity.value = Mathf.Lerp(chromAb.intensity.value,maxChromaticIntensity,0.5f*Time.deltaTime);
        FilmGr.intensity.value = Mathf.Lerp(FilmGr.intensity.value, maxFilmGrainIntensity, 0.5f * Time.deltaTime);
        Vign.intensity.value = Mathf.Lerp(Vign.intensity.value, maxVignetteIntensity, 0.5f * Time.deltaTime);
        LensDis.intensity.value = Mathf.Lerp(LensDis.intensity.value, maxLensDistance, 05f * Time.deltaTime);
        //play Hellhound damage screen overlay animation
    }

    void VisualFXEnd()
    {
        chromAb.intensity.value = minChromaticIntensity;
        FilmGr.intensity.value = minFilmGrainIntensity;
        Vign.intensity.value = minVignetteIntensity;
        LensDis.intensity.value = minLensDistance;
    }
}
