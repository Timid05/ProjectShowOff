using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    Camera _camera;
    GameObject carriedObject;
    GameObject duplicatedObject;
    [SerializeField] GameObject carriedObjectPosition;
    [SerializeField] float pickupDistance = 5;
    bool carryingObject = false;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo, pickupDistance))
        {
            //Debug.LogFormat("Carrying object: {0} Carried Object: {1}", carryingObject, carriedObject);
            // Pickup Object creates a duplicate of that object with only a meshrenderer.
            if (!carryingObject && carriedObjectPosition != null)
            {
                Debug.Log("Not carrying object.");
                if (hitinfo.collider.gameObject.CompareTag("PickUpObject"))
                {
                    Debug.Log("Clicked on PickupObject, duplicating.");
                    carryingObject = true;
                    carriedObject = hitinfo.collider.gameObject;
                    //duplicatedObject = carriedObject;
                    // Remove rigidbody from duplicated object.
                    Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        Debug.Log("Removing rigid body.");
                        DestroyImmediate(rb);
                    }

                    //Instantiate(duplicatedObject, carriedObjectPosition.transform.position, duplicatedObject.transform.rotation, _camera.transform);
                    //Destroy(hitinfo.collider.gameObject);
                    hitinfo.collider.gameObject.transform.position = carriedObjectPosition.transform.position;
                    hitinfo.collider.gameObject.transform.parent = _camera.transform;
                }
            }
            // Put down object
            else if (carryingObject && carriedObject != null)
            {
                Debug.Log("Putting down object.");
                carryingObject = false;
                carriedObject.AddComponent<Rigidbody>();
                carriedObject.transform.parent = null;
                carriedObject.transform.position = hitinfo.point;
                //Destroy(duplicatedObject);
                //Instantiate(carriedObject, Input.mousePosition, carriedObject.transform.rotation);
            }
        }

    }
}
