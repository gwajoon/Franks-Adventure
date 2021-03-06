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
    int currentQuestion = 0;

    List<QuestionBase> questions;

    bool isTrainerBattle = false;

    Monster Frank;

    Monster wildMonster;

    // initiate normal encounters
    public void StartBattle(Monster Frank, Monster wildMonster) 
    {
        this.Frank = Frank;
        this.wildMonster = wildMonster;
        questions = wildMonster.Base.Questions;
        StartCoroutine(SetupBattle());
    }

    // overloaded function to initiate trainer battle
    public void StartTrainerBattle(Monster Frank, TrainerController trainer) 
    {
        isTrainerBattle = true;
        this.Frank = Frank;
        wildMonster = trainer.Monster;
        wildMonster.Init();
        questions = wildMonster.Base.Questions;
        StartCoroutine(SetupBattle());
    }

    // setting of the HUD and dialogue
    public IEnumerator SetupBattle()
    {
        PlayerUnit.Setup(Frank);
        EnemyUnit.Setup(wildMonster);

        if (!isTrainerBattle)
        {
            yield return dialogueBox.TypeDialogue($"A wild {EnemyUnit.monster.Base.name} appeared.");
            yield return new WaitForSeconds(1f); 
        } else
        {
            yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.Name} has challenged you to a knowledge battle.");
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(EnemyQuestion());
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogueBox.setDialogue($"{EnemyUnit.monster.Base.QuestionName(currentQuestion)}?");
        dialogueBox.EnableActionSelector(true);
        dialogueBox.EnableDialogueText(true);
        dialogueBox.EnableAnswerSelector(false);
    }

    void AnswerSelection()
    {
        state = BattleState.AnswerSelection;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableAnswerSelector(true);
    }

    IEnumerator Hint()
    {
        yield return dialogueBox.TypeDialogue(questions[currentQuestion].Explanation);
        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var answer = questions[currentQuestion].Answers[currentAnswer].Name;
        yield return dialogueBox.TypeDialogue($"Frank answered {answer}.");
        yield return new WaitForSeconds(0.5f);
        
        if (answer == questions[currentQuestion].CorrectAnswer) 
        {
            PlayerUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(0.7f);
            EnemyUnit.PlayHitAnimation();

            bool isFainted = EnemyUnit.monster.TakeDamage(answer, PlayerUnit.monster);
            yield return EnemyUnit.Hud.UpdateHP();

            questions[currentQuestion].answered = true;

            if (isFainted)
            {
                yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.Name} fainted.");
                EnemyUnit.PlayFaintAnimation();

                if (isTrainerBattle) {
                    yield return dialogueBox.TypeDialogue($"You have defeated {EnemyUnit.monster.Base.Name}");
                }
                yield return new WaitForSeconds(1f);

                OnBattleOver(true);
                
                foreach (QuestionBase q in questions) {
                    q.answered = false;
                }
            }
            else
            { 
                while (questions[currentQuestion].Answered == true) {
                    currentQuestion++;    
                    if (currentQuestion == EnemyUnit.monster.Base.QuestionCount) {
                    currentQuestion = 0;                
                    }
                }

                yield return EnemyQuestion();
            }
        }
        // wrong answer
        else
        {
            yield return dialogueBox.TypeDialogue("Wrong answer!");
            yield return new WaitForSeconds(0.5f);

            EnemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(0.7f);
            PlayerUnit.PlayHitAnimation();

            yield return dialogueBox.TypeDialogue(questions[currentQuestion].Explanation);
            yield return new WaitForSeconds(0.5f);

            bool isFainted = PlayerUnit.monster.TakeDamage(answer, EnemyUnit.monster);
            yield return PlayerUnit.Hud.UpdateHP();

            if (isFainted)
            {
                if (EnemyUnit.monster.Base.Name == "Perry")
                {
                    yield return dialogueBox.TypeDialogue("MUAHAHAHA! You can't defeat me!");
                    yield return dialogueBox.TypeDialogue("Talk to me after this, I shall reveal the answer.");
                    yield return new WaitForSeconds(1f);
                    OnBattleOver(true);
                }
                else {
                    yield return dialogueBox.TypeDialogue($"Frank fainted.");
                    PlayerUnit.PlayFaintAnimation();

                    yield return new WaitForSeconds(1f);
                    OnBattleOver(false);
                }
            }
            else
            {
                while (questions[currentQuestion].Answered == true) {
                    currentQuestion++;
                    if (currentQuestion == EnemyUnit.monster.Base.QuestionCount) {
                    currentQuestion = 0;
                    } 
                }
                yield return EnemyQuestion();
            }
        }
    }

    IEnumerator EnemyQuestion()
    {
        state = BattleState.PerformMove;
        dialogueBox.SetAnswerNames(questions[currentQuestion]);
        yield return dialogueBox.TypeDialogue($"{EnemyUnit.monster.Base.Name} has a menacing question!");
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
                // Hint
                Hint();     
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
