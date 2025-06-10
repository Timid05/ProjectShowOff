using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundAnimations : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void TriggerCharge()
    {
        animator.SetBool("charging", true);
    }

    private void TriggerPounce()
    {
        animator.SetBool("pouncing", true);
    }

    private void StopAnimating()
    {
        animator.SetBool("charging", false);
        animator.SetBool("pouncing", false);
    }
}
