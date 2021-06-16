using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Sprite image;

    public void Interact(Transform initiator) {
        // Show text when player interact with object
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, image));
   }
}
