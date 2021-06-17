using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderHolder : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Sprite sprite;
    [SerializeField] string name;

    public void Interact(Transform initiator) {
        // Show text when player interact with object
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == name)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
