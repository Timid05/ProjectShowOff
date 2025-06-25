using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraOperator : MonoBehaviour
{
    [SerializeField]
    GameObject playerCam, tanfanaCam;
    DialogueRunner dialogueRunner;


    private void OnEnable()
    {
        dialogueRunner = GetComponent<GameManager>()._dialogueRunner;
        dialogueRunner.onDialogueStart.AddListener(ChangeToTanfanaCam);
        dialogueRunner.onDialogueComplete.AddListener(MoveCamToPlayer);
        CameraActions.OnCameraBackOnPlayer += ChangeToPlayerCam;
        CameraActions.OnTanfanaCamInit += AssignTanfanaCam;
    }

    private void OnDisable()
    {
        dialogueRunner.onDialogueStart.RemoveListener(ChangeToTanfanaCam);
        dialogueRunner.onDialogueComplete.RemoveListener(MoveCamToPlayer);
        CameraActions.OnCameraBackOnPlayer -= ChangeToPlayerCam;
        CameraActions.OnTanfanaCamInit += AssignTanfanaCam;
    }


    void ChangeToTanfanaCam()
    {
        playerCam.SetActive(false);
        tanfanaCam.SetActive(true);
        CameraActions.OnCameraMovingToNPC?.Invoke();
    }

    void MoveCamToPlayer()
    {
        CameraActions.OnCameraMovingToPlayer?.Invoke();
    }

    void ChangeToPlayerCam()
    {
        playerCam.SetActive(true);
        tanfanaCam.SetActive(false);
    }

    void AssignTanfanaCam(GameObject cam)
    {
        tanfanaCam = cam;
    }
}
