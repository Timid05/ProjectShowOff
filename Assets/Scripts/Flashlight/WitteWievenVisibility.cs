using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitteWievenVisibility : MonoBehaviour
{
    bool isFlashlightOn;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightActions.OnFlashlightStatusChange += ChangeVisibility;
        EnemiesInfo.OnEnemyAdded += ChangeVisibility;
    }

    void ChangeVisibility(bool flashlightOn)
    {
        isFlashlightOn = flashlightOn;
        //Change visibility to the opposite of the flashlight for all the Witte Wieven, who are children of this parent object.
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform witteWief = transform.GetChild(i);
            MeshRenderer mr = witteWief.gameObject.GetComponent<MeshRenderer>();
            mr.enabled = !flashlightOn;
        }
    }

    void ChangeVisibility()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform witteWief = transform.GetChild(i);
            MeshRenderer mr = witteWief.gameObject.GetComponent<MeshRenderer>();
            mr.enabled = !isFlashlightOn;
        }
    }

    private void OnDestroy()
    {
        FlashlightActions.OnFlashlightStatusChange -= ChangeVisibility;
    }
}
