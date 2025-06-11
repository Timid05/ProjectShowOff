using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    Camera _camera;
    [SerializeField] GameObject carriedObjectPosition;

    bool carryingObject = false;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            // Pickup Object
            if (!carryingObject && carriedObjectPosition != null && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo))
            {
                if (hitinfo.collider.gameObject.CompareTag("PickUpObject"))
                {
                    carryingObject = true;
                    hitinfo.collider.gameObject.transform.position = carriedObjectPosition.transform.position;
                    hitinfo.collider.gameObject.transform.parent = _camera.transform;
                }
            }

            // Put down Object

        }

    }
}
