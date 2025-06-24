using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlightpull : MonoBehaviour
{

    public Transform cameratransform;
    public float springsensitivity = 0.05f;
    public float springstrength = 5f;
    public float damping = 3f;

    private Vector3 originallocalposition;
    private Vector3 currentoffset;
    private Vector3 velocity;


    private void Start()
    {
        originallocalposition = transform.localPosition;
    }
    void Update()
    {
        if (cameratransform == null) return;

        Vector3 cameratilt = cameratransform.localEulerAngles;

        cameratilt.x = NormalizeAngle(cameratilt.x);
        cameratilt.z = NormalizeAngle(cameratilt.z);
        cameratilt.y = NormalizeAngle(cameratilt.y);

        Vector3 desiredoffset = new Vector3(cameratilt.z * springsensitivity +cameratilt.y*springsensitivity*0.4f, -cameratilt.x * springsensitivity,0);

        currentoffset = Vector3.SmoothDamp(currentoffset,desiredoffset, ref velocity, damping/springstrength);

        transform.localPosition = originallocalposition + currentoffset;

        float NormalizeAngle(float angle)
        {
            if (angle > 180f) angle -= 360f;
            return angle;
        }
    }
}
