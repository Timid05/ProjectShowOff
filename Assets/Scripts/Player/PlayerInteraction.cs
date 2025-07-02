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
    [SerializeField]
    GameObject interactUI;
    GameObject currentNPC;
    [SerializeField] KeyCode interactButton = KeyCode.E;
    public static event Action<bool> OnCharacterTalk;

    private void Awake()
    {
        GameManager.OnGiveGManager += ReceiveGManager;
        MapAnimation.OnMapButtonPressed += PlayerStatus;
       
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
            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
                if (OnCharacterTalk != null) { OnCharacterTalk(false); }
            }           
        }

        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo) && !dialogueRunner.IsDialogueRunning)
        {
            //Debug.LogFormat("Clicked button while free. Object: {0}", hitinfo.collider.gameObject.name);
            // Timescale is set to 0 so that the game is paused when in the menus. This can be used to prevent the startPos from talking to NPCs when they're in a menu.
            if (hitinfo.collider.gameObject.tag == "NPC" && Time.timeScale != 0f && !playerBusy)
            {

                interactUI.SetActive(true);
                //Debug.Log("Clicked on NPC.");
                if (Input.GetKeyUp(interactButton))
                {
                    if (OnCharacterTalk != null) { OnCharacterTalk(true); }
                    currentNPC = hitinfo.collider.gameObject;
                    currentNPC.GetComponent<NPCInteraction>().StartInteraction();
                    // This will prevent the startPos from using the flashlight while talking to NPCs.
                   
                }
            }
            else if (hitinfo.collider.gameObject.tag == "PickUpObject")
            {
                interactUI.SetActive(true);
            }
            else
            {
                interactUI.SetActive(false);
            }
        }
        else
        {
            interactUI.SetActive(false);
        }
    }

    // Allows script to receive the game manager in an efficient way without using FindObjectOfType
    void ReceiveGManager(GameManager gManager)
    {
        gameManager = gManager;
        dialogueRunner = gameManager._dialogueRunner;
        dialogueRunner.onDialogueComplete.AddListener(OnCompleteDialogue);
        dialogueRunner.onDialogueStart.AddListener(UnlockCursor);
        dialogueRunner.onDialogueStart.AddListener(OnStartDialogue);
        drImage = dialogueRunner.GetComponentInChildren<Image>();
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameStateActions.inDialogue = true;
    }

    public void OnCompleteDialogue()
    {
        Debug.Log("completing dialogue");
        GameStateActions.inDialogue = false;
        EnemiesInfo.OnShowEnemies?.Invoke();
        if (currentNPC != null)
        {
            if (!GameStateActions.domeVisited)
            {
                if (currentNPC.gameObject.name == "Tanfana")
                {
                    GameStateActions.OnFirstDomeVisit?.Invoke();
                    GameStateActions.domeVisited = true;
                }
            }
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
        EnemiesInfo.OnHideEnemies?.Invoke();
        playerMovement.SetEnabledMove(false);
        playerLook.SetEnabledLook(false);
        drImage.enabled = true;
        GameStateActions.OnNPCInteraction?.Invoke(currentNPC);
        //_light.intensity = 50000;
    }

    void PlayerStatus(bool pPlayerBusy)
    {
        playerBusy = pPlayerBusy;
    }

    private void OnDestroy()
    {
        GameManager.OnGiveGManager -= ReceiveGManager;
        MapAnimation.OnMapButtonPressed -= PlayerStatus;
        dialogueRunner.onDialogueComplete.RemoveListener(OnCompleteDialogue);
        dialogueRunner.onDialogueStart.RemoveListener(UnlockCursor);
        dialogueRunner.onDialogueStart.RemoveListener(OnStartDialogue);
    }
}