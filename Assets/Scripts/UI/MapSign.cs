using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSign : MonoBehaviour
{
    [SerializeField] UDictionary<GameObject, GameObject> signMapConversion;


    private void Awake()
    {
        SignClose.OnSignPlayerDistanceStatusChange += SwitchSignHighlight;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void SwitchSignHighlight(GameObject sign, bool highlight)
    {
        if(signMapConversion.ContainsKey(sign))
        {
            GameObject mapSign;
            signMapConversion.TryGetValue(sign, out mapSign);
            if(mapSign != null && mapSign.transform.childCount == 2)
            {
                Debug.LogFormat("Switching highlight to {0} for sign {1}.", sign, highlight);
                mapSign.transform.GetChild(0).gameObject.SetActive(!highlight);
                mapSign.transform.GetChild(1).gameObject.SetActive(highlight);
            }
        }
    }

    private void OnDestroy()
    {
        SignClose.OnSignPlayerDistanceStatusChange -= SwitchSignHighlight;
    }
}
