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
    bool playerBusy = false;

    public Light _light;
    public Camera _camera;
    GameObject currentNPC;
    public static event Action<bool> OnCharacterTalk;

    private void Awake()
    {
        GameManager.OnGiveGManager += ReceiveGManager;
        MapVisibility.OnMapButtonPressed += PlayerStatus;
    }

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerLook = gameObject.GetComponent<PlayerLook>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            dialogueRunner.Stop();
            if (OnCharacterTalk != null) { OnCharacterTalk(false); }
        }

        if (Input.GetMouseButtonUp(0) && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo) && !dialogueRunner.Dialogue.IsActive)
        {
            //Debug.LogFormat("Clicked button while free. Object: {0}", hitinfo.collider.gameObject.name);
            // Timescale is set to 0 so that the game is paused when in the menus. This can be used to prevent the player from talking to NPCs when they're in a menu.
            if (hitinfo.collider.gameObject.tag == "NPC" && Time.timeScale != 0f && !playerBusy)
            {
                //Debug.Log("Clicked on NPC.");
                currentNPC = hitinfo.collider.gameObject;
                currentNPC.GetComponent<NPCInteraction>().StartInteraction();
                // This will prevent the player from using the flashlight while talking to NPCs.
                if (OnCharacterTalk != null) { OnCharacterTalk(true); }
            }
        }
    }

    // Allows script to receive the game manager in an efficient way without using FindObjectOfType
    void ReceiveGManager(GameManager gManager)
    {
        gameManager = gManager;
        dialogueRunner = gameManager._dialogueRunner;
        drImage = dialogueRunner.GetComponentInChildren<Image>();
    }

    public void OnCompleteDialogue()
    {
        if (currentNPC != null)
        {
            Debug.Log($" ncp {currentNPC}");
            currentNPC = null;
        }

        if (!ResumeScene.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

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
        //Debug.Log("Start talking to NPC.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.SetEnabledMove(false);
        playerLook.SetEnabledLook(false);
        drImage.enabled = true;
        //_light.intensity = 50000;
    }

    void PlayerStatus(bool pPlayerBusy)
    {
        playerBusy = pPlayerBusy;
    }

    private void OnDestroy()
    {
        GameManager.OnGiveGManager -= ReceiveGManager;
        MapVisibility.OnMapButtonPressed -= PlayerStatus;
    }
}
