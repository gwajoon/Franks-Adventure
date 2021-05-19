using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject answerSelector;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> answerTexts;

    public void setDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogueText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableAnswerSelector(bool enabled)
    {
        answerSelector.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else 
                actionTexts[i].color = Color.black;
        }
    }

    public void UpdateAnswerSelection(int selectedAnswer)
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            if (i == selectedAnswer)
            {
                answerTexts[i].color = highlightedColor;
            }
            else 
                answerTexts[i].color = Color.black;
        }
    }

    // set the 4 options in the fight menu to the 4 answers available
    public void SetAnswerNames(QuestionBase Question)
    {
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = Question.Answers[i].Name;
        }
    } 
}
