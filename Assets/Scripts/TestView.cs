using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TestView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fpsCount;
    [SerializeField]
    private float fpsInterval = 0.1f;
    private int fpsSampleRate = 10;


    float lastDisplayTime = 0;
    float lastSampleTime = 0;
    int[] sampleFPS;
    int sampleCount = 0;
    private void DisplayFPS()
    {
        if (lastSampleTime == 0)
        {
            sampleFPS = new int[fpsSampleRate];
        }

        if (Time.time - lastSampleTime >= fpsInterval / fpsSampleRate)
        {
            sampleFPS[sampleCount] = (int)(1.0f / Time.deltaTime);
            lastSampleTime = Time.time;
            sampleCount++;
        }

        if (Time.time - lastDisplayTime >= fpsInterval)
        {
            float averageFPS = (int)sampleFPS.Average();
            fpsCount.text = "FPS: " + averageFPS;
            lastDisplayTime = Time.time;
            sampleCount = 0;
        }
    }


    private void Update()
    {
        DisplayFPS();
    }
}
