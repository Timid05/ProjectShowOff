using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    private GameManager _gameManager;
    public AudioClip audioClip;
    public Image image;
    public float size;
 
    void Start()
    {
        _gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }

    public void StartInteraction()
    {       
        _gameManager.StartInteraction(image.sprite, audioClip, size, name);
    }
}
