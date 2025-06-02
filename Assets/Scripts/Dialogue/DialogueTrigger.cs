using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        visualCue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.IsOwner)
            {
                visualCue.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.IsOwner)
            {
                visualCue.SetActive(false);
            }
        }
    }

    public TextAsset GetInk() { return inkJSON; }
}
