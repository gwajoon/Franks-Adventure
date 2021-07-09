using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Dialogue dialogueAfterBattle;
    [SerializeField] Monster monster;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject FOV;

    // state of trainer
    bool battleLost;
    Character character; 

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFOVRotation(character.Animator.DefaultDirection);
    }

    public void Interact(Transform initiator)
    {
        character.LookTowards(initiator.position); 

        if (!battleLost)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () => 
        {
            GameController.Instance.StartTrainerBattle(this);
        }));
        } else 
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogueAfterBattle));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        //flash the exclamation mark above trainer for 0.5s
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // make trainer walk towards player
        // diff.normalized is to get the trainer to move to 1 tile before player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // show dialogue
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () => 
        {
            GameController.Instance.StartTrainerBattle(this);
        }));
    }

    // after trainer lost, he won't challenge for battle again
    public void BattleLost()
    {
        battleLost = true;
        FOV.gameObject.SetActive(false);
    }

    private void Update()
    {
        character.HandleUpdate();
    }

    public void SetFOVRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        // set FOV of NPCs according to direction they face
        FOV.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        battleLost = (bool) state;

        if (battleLost)
            FOV.gameObject.SetActive(false);
    }

    public Monster Monster
    {
       get { return monster; }
    }
}
