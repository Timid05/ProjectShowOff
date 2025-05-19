using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitteWievenVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FlashlightActions.OnFlashlightStatusChange += ChangeVisibility;
    }

    void ChangeVisibility(bool flashlightOn)
    {
        //Change visibility to the opposite of the flashlight for all the Witte Wieven, who are children of this parent object.
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform witteWief = transform.GetChild(i);
            MeshRenderer mr = witteWief.gameObject.GetComponent<MeshRenderer>();
            mr.enabled = !flashlightOn;
        }
    }

    private void OnDestroy()
    {
        FlashlightActions.OnFlashlightStatusChange -= ChangeVisibility;
    }
}
