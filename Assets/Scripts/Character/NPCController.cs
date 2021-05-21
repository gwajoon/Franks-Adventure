using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] List<Sprite> sprites;

    SpriteAnimator spriteAnimator;

    private void Start()
    {
        spriteAnimator = new SpriteAnimator(GetComponent<SpriteRenderer>(), sprites);
        spriteAnimator.Start();
    }

    private void Update()
    {
        spriteAnimator.HandleUpdate();
    }

    public void Interact() {
       StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
   }
}
