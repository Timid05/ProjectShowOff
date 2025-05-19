using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    // Yarn spinner info / tips
    // https://www.youtube.com/watch?v=7nW8VlI3zOs
    // https://www.yarnspinner.dev/blog

    public GameObject _player;
    public DialogueRunner _dialogueRunner;
    public Camera _camera;
    AudioSource dialogueAS;

    public static event Action OnAcceptTanfanaChoice;
    public static event Action<GameManager> OnGiveGManager;

    public Dictionary<string, GameObject> _objects = new Dictionary<string, GameObject>();
    Dictionary<string, NPCInteraction> _NPCs;

    private void Awake()
    {
        // Receive the NPC dictionary once it's created.
        CreateNPCDict.OnDictCreated += ReceiveNPCs;
    }

    void Start()
    {
        dialogueAS = _dialogueRunner.GetComponentInChildren<AudioSource>();

        _dialogueRunner.AddFunction<string, bool>("PlayerMetNPC", PlayerMetNPC);
        _dialogueRunner.AddFunction<string, bool>("PlayerHasItem", PlayerHasItem);
        _dialogueRunner.AddFunction<string, bool>("PlayerGifItem", PlayerGifItem);
        _dialogueRunner.AddFunction<string, bool>("GoToNPC", GoToNPC);
        _dialogueRunner.AddFunction<string, bool>("GoToDialogue", GoToDialogue);
        _dialogueRunner.AddCommandHandler("TanfanaChoice", TanfanaChoice);

        // Send the game manager to scripts that need it.
        if(OnGiveGManager != null) { OnGiveGManager(this); }
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    void ReceiveNPCs(Dictionary<string, NPCInteraction> npcDict)
    {
        _NPCs = npcDict;
    }

    public void StartInteraction(Sprite image, AudioClip audioClip, float size, string name = null)
    {
        Debug.Log("test hier " + image + " / " + audioClip + " / " + name);

        if (name != null) _dialogueRunner.StartDialogue(name);
        _dialogueRunner.GetComponentInChildren<Image>().sprite = image;
        _dialogueRunner.GetComponentInChildren<Image>().GetComponent<Transform>().localScale = new Vector3(size, size, size);
        dialogueAS.clip = audioClip;
        dialogueAS.Play();
    }

    private bool PlayerMetNPC(string NPCName)
    {
        Debug.Log("checking npc " + NPCName);

        if (_objects.ContainsKey(NPCName))
        {
            return true;
        }
        else
        {
            _objects.Add(NPCName, _NPCs[NPCName].gameObject);
            return false;
        }
    }

    private bool PlayerHasItem(string item)
    {
        return (_objects.ContainsKey(item));
    }

    private bool PlayerGifItem(string item)
    {
        _objects.Remove(item);
        return true;
    }
    private bool GoToNPC(string NPCName)
    {
        StartInteraction(_NPCs[NPCName].image.sprite, _NPCs[NPCName].audioClip, _NPCs[NPCName].size);
        return true;
    }

    private bool GoToDialogue(string Dialogue)
    {
        if (name != null) _dialogueRunner.StartDialogue(Dialogue);
        dialogueAS.Play();
        return true;
    }
    private void TanfanaChoice()
    {
        if(OnAcceptTanfanaChoice != null) { OnAcceptTanfanaChoice(); }
    }

    private void OnDestroy()
    {
        CreateNPCDict.OnDictCreated -= ReceiveNPCs;
    }
}
