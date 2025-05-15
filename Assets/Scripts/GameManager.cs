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

    public static event Action OnAcceptTamfanaChoice;

    public Dictionary<string, GameObject> _objects = new Dictionary<string, GameObject>();
    public Dictionary<string, NPCInteraction> _NPC = new Dictionary<string, NPCInteraction>();

    void Start()
    {

        Transform[] allNPC = GameObject.FindGameObjectWithTag("AllNPC").GetComponentsInChildren<Transform>();
        if (allNPC != null)
        {
            foreach (var obj in allNPC)
            {
                if (obj.gameObject.tag == "NPC")
                {
                    Debug.Log(obj.gameObject.name);
                    _NPC.Add(obj.gameObject.name, obj.gameObject.GetComponent<NPCInteraction>());
                }
            }
        }

        _dialogueRunner.AddFunction<string, bool>("PlayerMetNPC", PlayerMetNPC);
        _dialogueRunner.AddFunction<string, bool>("PlayerHasItem", PlayerHasItem);
        _dialogueRunner.AddFunction<string, bool>("PlayerGifItem", PlayerGifItem);
        _dialogueRunner.AddFunction<string, bool>("GoToNPC", GoToNPC);
        _dialogueRunner.AddFunction<string, bool>("GoToDialogue", GoToDialogue);
        _dialogueRunner.AddCommandHandler("TamfanaChoice", TamfanaChoice);
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    public void StartInteraction(Sprite image, AudioClip audioClip, float size, string name = null)
    {
        Debug.Log("test hier " + image + " / " + audioClip + " / " + name);

        if (name != null) _dialogueRunner.StartDialogue(name);
        _dialogueRunner.GetComponentInChildren<Image>().sprite = image;
        _dialogueRunner.GetComponentInChildren<Image>().GetComponent<Transform>().localScale = new Vector3(size, size, size);
        _dialogueRunner.GetComponentInChildren<AudioSource>().clip = audioClip;
        _dialogueRunner.GetComponentInChildren<AudioSource>().Play();
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
            _objects.Add(NPCName, _NPC[NPCName].gameObject);
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
        StartInteraction(_NPC[NPCName].image.sprite, _NPC[NPCName].audioClip, _NPC[NPCName].size);
        return true;
    }

    private bool GoToDialogue(string Dialogue)
    {
        if (name != null) _dialogueRunner.StartDialogue(Dialogue);
        _dialogueRunner.GetComponentInChildren<AudioSource>().Play();
        return true;
    }

    //[YarnCommand("TamfanaChoice")]
    private void TamfanaChoice()
    {
        if(OnAcceptTamfanaChoice != null) { OnAcceptTamfanaChoice(); }
    }
}
