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
    private float maxChromaticIntensity = 70;
    private float minChromaticIntensity = 0;
    FilmGrain FilmGr;
    private float currentFilmGrainIntensity;
    private float maxFilmGrainIntensity = 100;
    private float minFilmGrainIntensity = 0;


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
        chromAb.intensity.value = Mathf.Lerp(chromAb.intensity.value,maxChromaticIntensity,0.2f*Time.deltaTime);
        FilmGr.intensity.value = Mathf.Lerp(FilmGr.intensity.value, maxFilmGrainIntensity, 0.2f * Time.deltaTime);
      
        //play Hellhound damage screen overlay animation
    }

    void VisualFXEnd()
    {
        chromAb.intensity.value = minChromaticIntensity;
        FilmGr.intensity.value = minFilmGrainIntensity;
    }
}
