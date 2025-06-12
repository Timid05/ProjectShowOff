using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightAnimations : MonoBehaviour
{
    Animator anim;

    private void OnEnable()
    {
        FlashlightActions.OnFlashlightStatusChange += Animate;
    }

    private void OnDisable()
    {
        FlashlightActions.OnFlashlightStatusChange -= Animate;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Animate(bool status)
    {
        if (status == true)
        {
            anim.SetTrigger("turnOn");
        }
        else
        {
            anim.SetTrigger("turnOff");
        }
    }
}
