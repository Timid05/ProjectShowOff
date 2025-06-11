using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundAnimations : MonoBehaviour
{
    Animator animator;
    AnimationClip currentClip;
    public static Action OnPounceAnimDone;

    private void OnEnable()
    {
        HellhoundActions.OnCharge += TriggerCharge;
        HellhoundActions.OnPounce += TriggerPounce;
    }

    private void OnDisable()
    {
        HellhoundActions.OnCharge += TriggerCharge;
        HellhoundActions.OnPounce += TriggerPounce;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void TriggerCharge()
    {
        animator.SetBool("pouncing", false);
        animator.SetBool("charging", true);
    }

    private void TriggerPounce()
    {
        animator.SetBool("charging", false);
        animator.SetBool("pouncing", true);
        StartCoroutine(Pouncing());
    }

    private IEnumerator Pouncing()
    {
        Debug.Log("Waiting till end");
        yield return new WaitForSeconds(GetCurrenStateInfo().length + 0.1f);
        Debug.Log("Invoking pounce end");
        OnPounceAnimDone?.Invoke();
        StopAnimating();
    }

    private void StopAnimating()
    {
        animator.SetBool("charging", false);
        animator.SetBool("pouncing", false);
    }

    AnimatorClipInfo GetCurrentClipInfo()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0];
    }

    AnimatorStateInfo GetCurrenStateInfo()
    {
        return animator.GetCurrentAnimatorStateInfo(0);  
    }
}
