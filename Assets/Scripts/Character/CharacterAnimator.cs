using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    
    // Parameters, the ones in animator tab
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    // States
    SpriteAnimator walkDownAnimation;
    SpriteAnimator walkUpAnimation;
    SpriteAnimator walkLeftAnimation;
    SpriteAnimator walkRightAnimation;

    SpriteAnimator currentAnimation;

    // References
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnimation = new SpriteAnimator(GetComponent<SpriteRenderer>(), walkDownSprites);
        walkUpAnimation = new SpriteAnimator(GetComponent<SpriteRenderer>(), walkUpSprites);
        walkLeftAnimation = new SpriteAnimator(GetComponent<SpriteRenderer>(), walkLeftSprites);
        walkRightAnimation = new SpriteAnimator(GetComponent<SpriteRenderer>(), walkRightSprites);
    
        currentAnimation = walkDownAnimation;
    }

    private void Update() 
    {
        var previousAnimation = currentAnimation;

        if (MoveX == 1)
            currentAnimation = walkRightAnimation;
        else if (MoveX == -1)
            currentAnimation = walkLeftAnimation;
        else if (MoveY == 1)
            currentAnimation = walkUpAnimation;
        else if (MoveY == -1)
            currentAnimation = walkDownAnimation;

        if (currentAnimation != previousAnimation || !IsMoving)
            currentAnimation.Start();

        if (IsMoving)
            currentAnimation.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnimation.Frames[0];
    }
}
