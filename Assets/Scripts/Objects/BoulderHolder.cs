using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderHolder : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Sprite sprite;
    [SerializeField] string name;

    public bool activated;

    void Start()
    {
        activated = false;
    }

    public void Interact(Transform initiator) {
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == name)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
            activated = true;
        }
    }
}
