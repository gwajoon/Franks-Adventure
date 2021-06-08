using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterAnimator animator; 
    public float moveSpeed;

    public bool IsMoving { get; private set; }

    public float OffsetY { get; private set; } = 0.3f;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    public void SetPositionAndSnapToTile(Vector2 pos) {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    public IEnumerator Move(Vector2 moveVec, Action onMoveOver=null)
    {
        // setting parameters of animator, mathclamp is to allow npcs to move > 1 tile
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        //calc target position
        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;
        
        // move character to target position only if tile is walkable
        if (!isPathClear (targetPos))
            yield break;
        
        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        onMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool isPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;

        // return a vector with the same direction but length of 1
        var dir = diff.normalized;

        // boxcast return true if there is a collider in the area of the box
        // origin(current pos of character), size, angle (0 cos we dw angle), direction and distance(length of diff)
        // + 1 to origin and -1 from magnitude (aka length) since we are not accounting for the character current tile
        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer))
            return false;
        
        return true;
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    // character will look towards vector3 targetPos being passed in
    public void LookTowards(Vector3 targetPos)
    {
        // floor to find integer value to find no.of tile differnce
        var xDiff =  Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var yDiff =  Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        // to ensure that character will only respond when the player is in the same x and y plane as him aka x || y == 0
        if (xDiff == 0 || yDiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
    }

    public CharacterAnimator Animator {
        get => animator;
    }
}
