using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System;

public class PlayerInteraction : MonoBehaviour
{
    private GameManager gameManager;
    private DialogueRunner dialogueRunner;
    Image drImage;
    PlayerMovement playerMovement;
    PlayerLook playerLook;

    public Light _light;
    public Camera _camera;
    GameObject currentNPC;
    public static event Action<bool> OnCharacterTalk;
    public Canvas _map;

    private void Awake()
    {
        GameManager.OnGiveGManager += ReceiveGManager;
    }

    void Start()
    {
        dialogueRunner = gameManager._dialogueRunner;
        drImage = dialogueRunner.GetComponentInChildren<Image>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerLook = gameObject.GetComponent<PlayerLook>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M) && !dialogueRunner.Dialogue.IsActive)
        {
            Debug.Log("test map " + _map.GetComponent<Canvas>().enabled);
            _map.GetComponent<Canvas>().enabled = !_map.GetComponent<Canvas>().enabled;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            dialogueRunner.Stop();
            if (OnCharacterTalk != null) { OnCharacterTalk(false); }
        }

        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo) && Input.GetMouseButtonUp(0) && !dialogueRunner.Dialogue.IsActive)
        {
            // Timescale is set to 0 so that the game is paused when in the menus. This can be used to prevent the player from talking to NPCs when they're in a menu.
            if (hitinfo.collider.gameObject.tag == "NPC" && Time.timeScale != 0f)
            {
                currentNPC = hitinfo.collider.gameObject;
                currentNPC.GetComponent<NPCInteraction>().StartInteraction();
                // This will prevent the player from using the flashlight while talking to NPCs.
                if(OnCharacterTalk != null) { OnCharacterTalk(true); }
            }
        }
    }

    // Allows script to receive the game manager in an efficient way without using FindObjectOfType
    void ReceiveGManager(GameManager gManager) { gameManager = gManager; }

    public void OnCompleteDialogue()
    {
        if (currentNPC != null)
        {
            Debug.Log($" ncp {currentNPC}");
            currentNPC = null;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.SetEnabledMove(true);
        playerLook.SetEnabledLook(true);
        drImage.enabled = false;
        dialogueRunner.GetComponentInChildren<AudioSource>().Stop();
        //_light.intensity = 130000;
        // Reenable the ability to use the flashlight.
        if (OnCharacterTalk != null) { OnCharacterTalk(false); }
    }

    public void OnStartDialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.SetEnabledMove(false);
        playerLook.SetEnabledLook(false);
        drImage.enabled = true;
        //_light.intensity = 50000;
    }

    private void OnDestroy()
    {
        GameManager.OnGiveGManager -= ReceiveGManager;
    }
}
