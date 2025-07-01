using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    Camera _camera;
    GameManager gameManager;
    GameObject carriedObject;
    [SerializeField] GameObject carriedObjectPosition;
    [SerializeField] float pickupDistance = 5;
    [SerializeField] KeyCode pickUpButton = KeyCode.E;
    [SerializeField] KeyCode putDownButton = KeyCode.Q;
    bool carryingObject = false;

    private void Awake()
    {
        GameManager.OnGiveGManager += ReceiveGameManager;
    }

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if(Input.GetKeyDown(pickUpButton)) { ObjectPlacement(pickUpButton); }
        else if (Input.GetKeyDown(putDownButton)) { ObjectPlacement(putDownButton); }

        // Check if an object was given to Tanfana
        if(gameManager != null && carryingObject && !gameManager._objects.ContainsKey(carriedObject.name))
        {
            Debug.LogFormat("Object {0} was gifted to an Tanfana.", carriedObject.name);
            carryingObject = false;
            Destroy(carriedObject);
        }
    }

    void ObjectPlacement(KeyCode buttonPressed)
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo, pickupDistance))
        {
            // Pickup Object creates a duplicate of that object with only a meshrenderer.
            if (buttonPressed == pickUpButton && !carryingObject && carriedObjectPosition != null)
            {
                Debug.LogFormat("Not carrying object. Clicked on object {0} with tag {1} ", hitinfo.collider.gameObject.name, hitinfo.collider.gameObject.tag);
                if (hitinfo.collider.gameObject.CompareTag("PickUpObject"))
                {
                    GameStateActions.OnChaliceCollected?.Invoke();
                    //Debug.Log("Clicked on PickupObject, duplicating.");
                    carryingObject = true;
                    carriedObject = hitinfo.collider.gameObject;

                    // Remove rigidbody from duplicated object.
                    Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        //Debug.Log("Removing rigid body.");
                        DestroyImmediate(rb);
                    }

                    hitinfo.collider.gameObject.transform.parent = _camera.transform;
                    hitinfo.collider.gameObject.transform.position = carriedObjectPosition.transform.position;
                    hitinfo.collider.gameObject.transform.rotation = carriedObjectPosition.transform.rotation;

                    // This lets the game manager know we have the object.
                    if (gameManager != null) { gameManager._objects.Add(carriedObject.name, carriedObject); }
                }
            }
            // Put down object
            else if (buttonPressed == putDownButton && carryingObject && carriedObject != null)
            {
                Debug.Log("Putting down object.");
                carryingObject = false;
                carriedObject.AddComponent<Rigidbody>();
                carriedObject.transform.parent = null;
                carriedObject.transform.position = hitinfo.point;

                // Remove the object from the objects the player has.
                if (gameManager != null) { gameManager._objects.Remove(carriedObject.name); }
            }
        }
    }

    void ReceiveGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    private void OnDestroy()
    {
        GameManager.OnGiveGManager -= ReceiveGameManager;
    }
}
