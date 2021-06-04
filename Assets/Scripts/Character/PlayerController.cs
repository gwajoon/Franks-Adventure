using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Monster Frank;

    private Vector2 input;
    private Character character;

    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainerView;   

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        Frank.Init();
    }

    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, onMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
            Interact();
    }

    void Interact() 
    {
        var facingDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPosition = transform.position + facingDirection;

       // Debug.DrawLine(transform.position, interactPosition, Color.green, 0.5f);

       var collider = Physics2D.OverlapCircle(interactPosition, 0.3f, GameLayers.i.InteractableLayer);
       if (collider != null) 
       {
           collider.GetComponent<Interactable>()?.Interact(transform);
       }
    }  

    private void onMoveOver()
    {
        // // return an array which represents all the game objects which player overlaps with
        // var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, offsetY), 0.2f, GameLayers.i.TriggerableLayer);
    
        // foreach (var collider in colliders)
        // {
        //     var triggerable = collider.GetComponent<IPlayerTriggerable>();
        //     // get all colliders with triggerable interface
        //     if (triggerable != null)
        //     {
        //         triggerable.onPlayerTriggered(this);
        //         break;
        //     }
        // }
        checkForEncounters();
        checkTrainerView();
    }

    private void checkTrainerView()
    {
        var trainer = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FOVLayer);
        if (trainer != null)
        {
            character.Animator.IsMoving = false;
            OnEnterTrainerView?.Invoke(trainer);
        }
    }

    // random encounters with monsters in the wild
    private void checkForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                character.Animator.IsMoving = false;
                OnEncountered();
            }
        }
    }
}
