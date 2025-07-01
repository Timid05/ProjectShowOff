using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(anim != null) { anim.SetTrigger("mapOpen"); }
    }

    private void OnDisable()
    {
        if(anim != null) { anim.SetTrigger("mapClose"); }
    }
}
