using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Monster/Create new question")]

public class QuestionBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string explanation;
    [SerializeField] List<AnswerBase> answers;
    [SerializeField] AnswerBase correctAnswer;

    public string Name {
        get { return name; }
    }

    public string Explanation {
        get { return explanation; }
    }

    public List<AnswerBase> Answers {
        get { return answers; }
    }

    public string CorrectAnswer
    {
        get { return correctAnswer.Name; }
    }
}
