using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnEncountered;
    private Vector2 input;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
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
                StartCoroutine(character.Move(input, checkForEncounters));
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
           collider.GetComponent<Interactable>()?.Interact();
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
