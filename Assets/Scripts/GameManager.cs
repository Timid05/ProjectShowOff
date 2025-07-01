using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

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

    public bool hasPlayedFirstLine = false;

    private void Awake()
    {
        // Receive the NPC dictionary once it's created.
        Time.timeScale = 1.0f;
        CreateNPCDict.OnDictCreated += ReceiveNPCs;
        _dialogueRunner.AddCommandHandler<string>("playVoice", PlayVoiceClip);
    }

    void Start()
    {
        dialogueAS = _dialogueRunner.GetComponentInChildren<AudioSource>();
       

        _dialogueRunner.AddFunction<string, bool>("PlayerMetNPC", PlayerMetNPC);
        _dialogueRunner.AddFunction<string, bool>("PlayerHasItem", PlayerHasItem);
        _dialogueRunner.AddFunction<string, bool>("PlayerGiftItem", PlayerGiftItem);
        _dialogueRunner.AddFunction<string, bool>("GoToNPC", GoToNPC);
        _dialogueRunner.AddFunction<string, bool>("GoToDialogue", GoToDialogue);
        _dialogueRunner.AddCommandHandler("TanfanaChoice", TanfanaChoice);

        // Send the game manager to scripts that need it.
        if (OnGiveGManager != null) { OnGiveGManager(this); }
    }

    private void OnEnable()
    {
        HellhoundActions.OnHellhoundDeath += ReachedEnd;
    }

    private void OnDisable()
    {
        HellhoundActions.OnHellhoundDeath -= ReachedEnd;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }

    void ReceiveNPCs(Dictionary<string, NPCInteraction> npcDict)
    {
        _NPCs = npcDict;
        // Send the game manager to the NPCs.
        if (OnGiveGManager != null) { OnGiveGManager(this); }
    }

    public void StartInteraction(Sprite image, AudioClip audioClip, float size, string name = null)
    {
        //Debug.Log("test hier " + image + " / " + audioClip + " / " + name);

        //Debug.Log("StartInteraction was called with: " + name);
        //Debug.Log("AudioClip assigned: " + (audioClip != null ? audioClip.name : "NULL"));

        if (name != null) _dialogueRunner.StartDialogue(name);
        _dialogueRunner.GetComponentInChildren<Image>().sprite = image;
        _dialogueRunner.GetComponentInChildren<Image>().GetComponent<Transform>().localScale = new Vector3(size, size, size);
        //dialogueAS.clip = audioClip;
        //dialogueAS.Play();
    }

    private bool PlayerMetNPC(string NPCName)
    {
        //Debug.Log("checking npc " + NPCName);

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
        if (_objects.ContainsKey(item))
        {
            return true;
        }
        return false;
    }

    private bool PlayerGiftItem(string item)
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
        EnemiesInfo.OnShowEnemies?.Invoke();
        if (OnAcceptTanfanaChoice != null) { OnAcceptTanfanaChoice(); }
        GameStateActions.OnChoiceMade?.Invoke(true);
        GameStateActions.acceptedChoice = true;
    }

    private void OnDestroy()
    {
        hasPlayedFirstLine = false;
        CreateNPCDict.OnDictCreated -= ReceiveNPCs;
    }

    [YarnCommand("ReturnChalice")]
    public void ReturnChalice()
    {
        Debug.Log("Yarn called ReturnChalice");
        GameStateActions.OnChaliceReturned?.Invoke();
    }

    [YarnCommand("Refused")]
    public void RefusedOffer()
    {
        EnemiesInfo.OnShowEnemies?.Invoke();
        GameStateActions.OnChoiceMade?.Invoke(false);
        GameStateActions.acceptedChoice = false;
    }

    private void PlayVoiceClip(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Voice/" + clipName);
        if (clip != null)
        {
            if (!hasPlayedFirstLine)
            {
                hasPlayedFirstLine = true;
                StartCoroutine(PlayClipDelayed(clip, 0.1f));
            } else
            {

                dialogueAS.clip = clip;
                dialogueAS.Play();
                Debug.Log("Playing voice clip: " + clipName);
            }
        }
        else
        {
            Debug.LogWarning("Voice clip not found: " + clipName);
        }
    }

    private IEnumerator PlayClipDelayed(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueAS.clip = clip;
        dialogueAS.Play();
        Debug.Log("Playing voice clip (delayed): " + clip.name);
    }

    void ReachedEnd()
    {
        if (GameStateActions.acceptedChoice)
        {
            StartCoroutine(WaitForComic(true));
        }
        else
        {
            StartCoroutine(WaitForComic(false));
        }
    }

    IEnumerator WaitForComic(bool accepted)
    {
        yield return new WaitForSeconds(5f);
        if (accepted)
        {         
            SceneManager.LoadScene("End Comic Bad");
            GameStateActions.Reset();
        }
        else
        {
            SceneManager.LoadScene("End Comic Good");
            GameStateActions.Reset();
        }
    }

}
