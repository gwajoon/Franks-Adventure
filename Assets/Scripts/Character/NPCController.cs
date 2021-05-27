using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState { Idle, Walking, Dialogue }

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

    // transform that initiate the interaction
    public void Interact(Transform initiator) {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialogue;
            character.LookTowards(initiator.position);

            // NPC state to go back to idle once dialogue is over
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () => {
                idleTimer = 0f;
                state = NPCState.Idle;
            }));
        }
   }

   private void Update()
   {
       // only runs when  NPC is idle so NPC that is talked to will stop moving
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

        var oldPosition = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        // check if the current position = previous position
        // if equals means the character did not move and is currently blocked by player, 
        // so it should not execute the next pattern of walking until it finishes this pattern first
        if (transform.position != oldPosition)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
   }
}

