using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanfanaCamera : MonoBehaviour
{
    [SerializeField]
    float panTime = 2f;
    float currentPanTime = 0;

    [SerializeField]
    Transform playerCam, tanfanaCam;

    Vector3 playerPos;
    Quaternion playerRot;
    Vector3 tanfanaPos;
    Quaternion tanfanaRot;

    Vector3 endPos;
    Vector3 startPos;
    Quaternion endRot;
    Quaternion startRot;
    bool moving = false;

    private void OnEnable()
    {  
        CameraActions.OnCameraMovingToPlayer += SetToPlayer;
        CameraActions.OnCameraMovingToNPC += SetToTanfana;
    }

    private void OnDisable()
    {
        CameraActions.OnCameraMovingToPlayer -= SetToPlayer;
        CameraActions.OnCameraMovingToNPC -= SetToTanfana;
    }

    private void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam").transform;
        playerPos = playerCam.position;
        playerRot = playerCam.rotation;
        tanfanaPos = tanfanaCam.position;
        tanfanaRot = tanfanaCam.rotation;
        CameraActions.OnTanfanaCamInit?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (moving)
        {
            if (currentPanTime >= panTime)
            {
                moving = false;
                currentPanTime = 0;

                if (endPos == playerPos)
                {
                    CameraActions.OnCameraBackOnPlayer?.Invoke();
                }

                return;
            }

            currentPanTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, currentPanTime * 1 / panTime);
            transform.rotation = Quaternion.Lerp(startRot, endRot, currentPanTime * 1 / panTime);
        }
    }

    void SetToPlayer()
    {
        UpdatePlayerPos();
        endPos = playerPos;
        startPos = tanfanaPos;
        startRot = tanfanaRot;
        endRot = playerRot;
        moving = true;
    }

    void SetToTanfana()
    {
        UpdatePlayerPos();
        endPos = tanfanaPos;
        startPos = playerPos;
        endRot = tanfanaRot;
        startRot = playerRot;
        moving = true;
    }

    void UpdatePlayerPos()
    {
        playerPos = playerCam.position;
        playerRot = playerCam.rotation;
    }
}
