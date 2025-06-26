using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ChaliceToggle : MonoBehaviour
{
    [SerializeField]
    GameObject chalice;
    static string chaliceName = "";

    private void OnEnable()
    {
        GameStateActions.OnChaliceReturned+= HideChalice;
    }

    private void OnDisable()
    {
        GameStateActions.OnChaliceReturned -= HideChalice;
    }

    void Start()
    {
        if (chalice != null)
        {
            chaliceName = chalice.name;
        }
    }

    void HideChalice()
    {
        if (chalice != null)
        {
            chalice.SetActive(false);
        }
    }

    [YarnFunction("GetChaliceName")]
    public static string GetChaliceName() 
    { 
        //Debug.Log("Chalice name is: " + chaliceName);
        return chaliceName;
    }
}
