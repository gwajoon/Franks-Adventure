using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState { Idle, Walking }

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTimer = 0f; 
    int currentPattern = 0;


    Character character;
   
   private void Awake()
   {
       character = GetComponent<Character>();
   }

    public void Interact() {
        if (state == NPCState.Idle)
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
   }

   private void Update()
   {
       if (DialogueManager.Instance.isShowing) 
            return;

       if (state == NPCState.Idle)
       {
           idleTimer += Time.deltaTime;
           // NPC move every 2 seconds
           if (idleTimer > timeBetweenPattern)
           {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
           }
       }
       character.HandleUpdate();
   }

   IEnumerator Walk()
   {
        state = NPCState.Walking;
        yield return character.Move(movementPattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movementPattern.Count;
        state = NPCState.Idle;
   }
}

