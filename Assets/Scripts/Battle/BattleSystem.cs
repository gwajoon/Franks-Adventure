using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// busy state is when player/enemy are attacking
public enum BattleState { Start, PlayerAction, PlayerAnswer, EnemyQuestion, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit FrankUnit;
    [SerializeField] BattleUnit EnemyUnit;
    [SerializeField] BattleHud FrankHud;
    [SerializeField] BattleHud EnemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentAnswer;

    int currentQuestion;

    public void StartBattle() 
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        FrankUnit.Setup();
        EnemyUnit.Setup();
        FrankHud.setData(FrankUnit.monster);
        EnemyHud.setData(EnemyUnit.monster);

        yield return dialogueBox.TypeDialogue($"A wild {EnemyUnit.monster.Base.name} appeared.");
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(EnemyQuestion());
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogueBox.setDialogue($"{EnemyUnit.monster.Base.QuestionName(currentQuestion)}?");
        dialogueBox.EnableActionSelector(true);
    }

    void PlayerAnswer()
    {
        state = BattleState.PlayerAnswer;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableAnswerSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var answer = EnemyUnit.monster.Base.Question(currentQuestion).Answers[currentAnswer].Name;
        yield return dialogueBox.TypeDialogue($"Frank answered {answer}.");
        yield return new WaitForSeconds(0.5f);
        
        if (answer == EnemyUnit.monster.Base.Question(currentQuestion).CorrectAnswer) 
        {
            FrankUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            EnemyUnit.PlayHitAnimation();

            bool isFainted = EnemyUnit.monster.TakeDamage(answer, FrankUnit.monster);
            yield return EnemyHud.UpdateHP();

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
            yield return new WaitForSeconds(1f);

            EnemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            FrankUnit.PlayHitAnimation();

            bool isFainted = FrankUnit.monster.TakeDamage(answer, EnemyUnit.monster);
            yield return FrankHud.UpdateHP();

            if (isFainted)
            {
                yield return dialogueBox.TypeDialogue($"Frank fainted.");
                FrankUnit.PlayFaintAnimation();

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
        state = BattleState.EnemyQuestion;
        dialogueBox.SetAnswerNames(EnemyUnit.monster.Base.Question(currentQuestion));
        yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.name} has a menacing question!");
        yield return new WaitForSeconds(1f);

        yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.QuestionName(currentQuestion)}?");
        yield return new WaitForSeconds(2f);

        PlayerAction();
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerAnswer)
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                // Fight
                PlayerAnswer();
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
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerAction();
        }

        dialogueBox.UpdateAnswerSelection(currentAnswer);
    }
}
