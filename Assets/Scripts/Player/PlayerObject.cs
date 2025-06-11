using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    Camera _camera;
    [SerializeField] GameObject carriedObject;
    bool carryingObject = false;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (!carryingObject && carriedObject != null && Input.GetMouseButtonUp(0) && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo)) 
        {
            if(hitinfo.collider.gameObject.CompareTag("PickUpObject"))
            {
                // Pickup Object
                carryingObject = true;
                hitinfo.collider.gameObject.transform.position = carriedObject.transform.position;
                hitinfo.collider.gameObject.transform.parent = _camera.transform;
            }
        }
    }
}
