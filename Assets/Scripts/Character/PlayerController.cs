using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISavable
{
    [SerializeField] Monster Frank;

    private Vector2 input;
    private Character character;

    public Monster getMonster
    {
        get => Frank;
    }

    public Character Character => character;

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
                StartCoroutine(character.Move(input, OnMoveOver));
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

        var collider = Physics2D.OverlapCircle(interactPosition, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null) 
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }

        var collider2 = Physics2D.OverlapCircle(interactPosition, 0.3f, GameLayers.i.ObjectLayer);
        if (collider2 != null) 
        {
            collider2.GetComponent<Interactable>()?.Interact(transform);
        }
    }  

    private void OnMoveOver()
    {
        // return an array which represents all the game objects which player overlaps with
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.OffsetY), 0.2f, GameLayers.i.TriggerableLayer);
    
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            // get all colliders with triggerable interface
            if (triggerable != null)
            {
                character.Animator.IsMoving = false;
                triggerable.OnPlayerTriggered(this);          
                break;
            }
        }
    }

    public object CaptureState()
    {
        float[] position = new float[] { transform.position.x, transform.position.y };
        return position;
    }

    public void RestoreState(object state)
    {
        var position = (float[])state;
        transform.position = new Vector3(position[0], position[1]);
    }
}
