using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// busy state is when player/enemy are attacking
public enum BattleState { Start, ActionSelection, AnswerSelection, PerformMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit PlayerUnit;
    [SerializeField] BattleUnit EnemyUnit;
    [SerializeField] BattleDialogueBox dialogueBox;
 
    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentAnswer;
    int currentQuestion;

    bool isTrainerBattle = false;

    Monster Frank;

    Monster wildMonster;

    public void StartBattle(Monster Frank, Monster wildMonster) 
    {
        this.Frank = Frank;
        this.wildMonster = wildMonster;
        StartCoroutine(SetupBattle());
    }

    public void StartTrainerBattle(TrainerController trainer) 
    {
        isTrainerBattle = true;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
            // playerUnit.Clear();
            // EnemyUnit.Clear();

            PlayerUnit.Setup(Frank);
            EnemyUnit.Setup(wildMonster);

        if (!isTrainerBattle)
        {
            yield return dialogueBox.TypeDialogue($"A wild {EnemyUnit.monster.Base.name} appeared.");
            yield return new WaitForSeconds(1f); 
        } else
        {
            yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.name} has challenged you to a knowledge battle.");
            yield return new WaitForSeconds(1f);

        }
        StartCoroutine(EnemyQuestion());
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogueBox.setDialogue($"{EnemyUnit.monster.Base.QuestionName(currentQuestion)}?");
        dialogueBox.EnableActionSelector(true);
    }

    void AnswerSelection()
    {
        state = BattleState.AnswerSelection;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableAnswerSelector(true);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var answer = EnemyUnit.monster.Base.Question(currentQuestion).Answers[currentAnswer].Name;
        yield return dialogueBox.TypeDialogue($"Frank answered {answer}.");
        yield return new WaitForSeconds(0.5f);
        
        if (answer == EnemyUnit.monster.Base.Question(currentQuestion).CorrectAnswer) 
        {
            PlayerUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(0.7f);
            EnemyUnit.PlayHitAnimation();

            bool isFainted = EnemyUnit.monster.TakeDamage(answer, PlayerUnit.monster);
            yield return EnemyUnit.Hud.UpdateHP();

            if (isFainted)
            {
                yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.Name} fainted.");
                EnemyUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(1f);
                OnBattleOver(true);
            }
            else
            {
                if (currentQuestion++ == EnemyUnit.monster.Base.QuestionCount - 1) {
                    currentQuestion = 0;
                } 
                yield return EnemyQuestion();
            }
        }
        else
        {
            yield return dialogueBox.TypeDialogue("Wrong answer!");
            yield return new WaitForSeconds(0.5f);

            EnemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(0.7f);
            PlayerUnit.PlayHitAnimation();

            bool isFainted = PlayerUnit.monster.TakeDamage(answer, EnemyUnit.monster);
            yield return PlayerUnit.Hud.UpdateHP();

            if (isFainted)
            {
                yield return dialogueBox.TypeDialogue($"Frank fainted.");
                PlayerUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(1f);
                OnBattleOver(false);
            }
            else
            {
                if (currentQuestion++ == EnemyUnit.monster.Base.QuestionCount - 1) {
                    currentQuestion = 0;
                } 
                yield return EnemyQuestion();
            }
        }
    }

    IEnumerator EnemyQuestion()
    {
        state = BattleState.PerformMove;
        dialogueBox.SetAnswerNames(EnemyUnit.monster.Base.Question(currentQuestion));
        yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.name} has a menacing question!");
        yield return new WaitForSeconds(1f);

        yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.QuestionName(currentQuestion)}?");
        yield return new WaitForSeconds(2f);

        ActionSelection();
    }

    private void Update()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.AnswerSelection)
        {
            HandleAnswerSelection();
        }
    }

    private void HandleActionSelection()
    {
        // increase or decrease currentAction which is either "fight" or "run"
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                currentAction--;
        }

        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAction == 0)
            {
                // Fight
                AnswerSelection();
            }
            else if (currentAction == 1) 
            {
                // Run

            }
        }
    }

    private void HandleAnswerSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAnswer < 5)
                currentAnswer++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAnswer > 0)
                currentAnswer--;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAnswer < 2)
                currentAnswer+=2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAnswer > 1)
                currentAnswer-=2;
        }   
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueBox.EnableAnswerSelector(false);
            dialogueBox.EnableDialogueText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActionSelection();
        }

        dialogueBox.UpdateAnswerSelection(currentAnswer);
    }
}
