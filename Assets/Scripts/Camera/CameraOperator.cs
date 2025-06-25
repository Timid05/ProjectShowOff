using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraOperator : MonoBehaviour
{
    [SerializeField]
    GameObject playerCam;
    GameObject tanfanaCam;
    DialogueRunner dialogueRunner;

    private void Awake()
    {
        TanfanaCamera.OnTanfanaCameraSpawn += GetTanfanaCamera;
    }

    private void OnEnable()
    {
        dialogueRunner = GetComponent<GameManager>()._dialogueRunner;
        dialogueRunner.onDialogueStart.AddListener(ChangeToTanfanaCam);
        dialogueRunner.onDialogueComplete.AddListener(MoveCamToPlayer);
        CameraActions.OnCameraBackOnPlayer += ChangeToPlayerCam;
    }

    private void OnDisable()
    {
        dialogueRunner.onDialogueStart.RemoveListener(ChangeToTanfanaCam);
        dialogueRunner.onDialogueComplete.RemoveListener(MoveCamToPlayer);
        CameraActions.OnCameraBackOnPlayer -= ChangeToPlayerCam;
    }


    void ChangeToTanfanaCam()
    {
        if (playerCam != null && tanfanaCam != null)
        {
            playerCam.SetActive(false);
            tanfanaCam.SetActive(true);
            CameraActions.OnCameraMovingToNPC?.Invoke();
        }
    }

    void MoveCamToPlayer()
    {
        CameraActions.OnCameraMovingToPlayer?.Invoke();
    }

    void ChangeToPlayerCam()
    {
        if (playerCam != null && tanfanaCam != null)
        {
            playerCam.SetActive(true);
            tanfanaCam.SetActive(false);
        }
    }

    void GetTanfanaCamera(GameObject pTanfanaCamera)
    {
        Debug.Log("Camera given tanfana camera.");
        tanfanaCam = pTanfanaCamera;
    }

    private void OnDestroy()
    {
        TanfanaCamera.OnTanfanaCameraSpawn -= GetTanfanaCamera;
    }
}
