using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellHoundFightBehavior : MonoBehaviour
{
    Transform player;
    bool fightOngoing = false;

    [SerializeField]
    float minGrowlInterval, maxGrowlInterval;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
