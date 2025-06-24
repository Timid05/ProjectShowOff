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
        GameStateActions.OnFirstDomeVisit += ShowChalice;
        GameStateActions.OnChaliceReturned+= HideChalice;
    }

    private void OnDisable()
    {
        GameStateActions.OnFirstDomeVisit -= ShowChalice;
        GameStateActions.OnChaliceReturned -= HideChalice;
    }

    void Start()
    {
        if (chalice != null)
        {
            chalice.SetActive(false);
            chaliceName = chalice.name;
        }
    }

    void ShowChalice()
    {
        if (chalice != null)
        {
            chalice.SetActive(true);
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
