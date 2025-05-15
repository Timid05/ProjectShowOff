using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdateAmplitude : MonoBehaviour
{
    CinemachineVirtualCamera vCamera;
    CinemachineBasicMultiChannelPerlin cameraNoise;
    float noiseMoveAmplitude = 1;
    [SerializeField] float noiseSprintAmplitude = 5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.OnMoveStatusChange += AmplitudeChange;

        vCamera = GetComponent<CinemachineVirtualCamera>();
        cameraNoise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // The regular move amplitude is the starting amplitude.
        noiseMoveAmplitude = cameraNoise.m_AmplitudeGain;
    }

    void AmplitudeChange(bool sprintStatus)
    {
        if(sprintStatus) { cameraNoise.m_AmplitudeGain = noiseMoveAmplitude; }
        else { cameraNoise.m_AmplitudeGain = noiseSprintAmplitude; }
        //Debug.Log("Set amplitude to: " + cameraNoise.m_AmplitudeGain);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnMoveStatusChange -= AmplitudeChange;
    }
}
