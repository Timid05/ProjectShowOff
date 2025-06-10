using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    Camera _camera;
    public Vector3 carriedObjectPosition;
    bool carryingObject = false;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (!carryingObject && Input.GetMouseButtonUp(0) && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo)) 
        {
            if(hitinfo.collider.gameObject.CompareTag("PickUpObject"))
            {
                // Pickup Object
                carryingObject = true;
                hitinfo.collider.gameObject.transform.position = carriedObjectPosition;
                hitinfo.collider.gameObject.transform.parent = gameObject.transform;
            }
        }
    }
}
