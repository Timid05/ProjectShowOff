using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    private GameManager _gameManager;
    public AudioClip audioClip;
    public Image image;
    public float size;

    private void Awake()
    {
        GameManager.OnGiveGManager += ReceiveGManager;
    }

    public void StartInteraction()
    {       
        _gameManager.StartInteraction(image.sprite, audioClip, size, name);
    }

    void ReceiveGManager(GameManager gManager) { _gameManager = gManager; }

    private void OnDestroy()
    {
        GameManager.OnGiveGManager -= ReceiveGManager;
    }
}
