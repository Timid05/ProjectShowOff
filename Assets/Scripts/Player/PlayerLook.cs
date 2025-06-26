using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float sensX = 150;
    [SerializeField] float sensY = 150;
    [SerializeField] float mouseXClamp = 6;
    [SerializeField] float mouseYClamp = 6;
    [SerializeField] Transform PlayerOrientation;

    float xRotation;
    float yRotation;
    float backUpsensX; 
    float backUpsensY;

    private void OnEnable()
    {
        PlayerActions.OnPlayerDead += DisableLook;
        GameStateActions.OnRespawnEnemies += EnableLook;
    }

    private void OnDisable()
    {
        PlayerActions.OnPlayerDead -= DisableLook;
        GameStateActions.OnRespawnEnemies -= EnableLook;
    }

    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        // Setting a default value incase
        if(sensX == 0 || sensY == 0)
        {
            backUpsensX = 150;
            backUpsensY = 150;
        }
        else
        {
            backUpsensX = sensX;
            backUpsensY = sensY;
        }
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
    }


    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;
        // Limiting the effect of time.deltaTime because it was making the look movement too fast when FPS was low.
        mouseX = Mathf.Clamp(mouseX * Time.deltaTime, -mouseXClamp, mouseXClamp);
        mouseY = Mathf.Clamp(mouseY * Time.deltaTime, -mouseYClamp, mouseYClamp);

        yRotation += mouseX;
        xRotation -= mouseY;
        //Debug.LogFormat("X rotation: {0} Y rotation: {1}", xRotation, yRotation);
        //Debug.LogFormat("Mouse X movement: {0} Mouse Y movement {1}", mouseX, mouseY);

        //To make it easier for the player to turn around.
        //if() { }

        xRotation = Mathf.Clamp(xRotation, -90f, 70f);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        PlayerOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //Debug.Log("Player Orientation: " + PlayerOrientation.rotation);
    }

    public void SetSensitivity(float sensitivity)
    {
        sensX = sensitivity;
        sensY = sensitivity;
        backUpsensX = sensX;
        backUpsensY = sensY;
    }

    void EnableLook()
    {
        SetEnabledLook(true);
    }

    private void DisableLook()
    {
        SetEnabledLook(false);
    }

    public void SetEnabledLook(bool enable)
    {
        if (enable)
        {
            sensX = backUpsensX;
            sensY = backUpsensY;
        }
        else
        {
            sensX = 0;
            sensY = 0;
        }
    }
}
