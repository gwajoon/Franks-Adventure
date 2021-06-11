using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]

public class MonsterBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    // randomise this in the future when there are more questions
    [SerializeField] List<QuestionBase> questions;

    [SerializeField] Sprite sprite;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public int Attack {
        get { return attack; }
    }

    public Sprite Sprite {
        get { return sprite; }
    }

    public string QuestionName(int index) {
        return questions[index].name;
    }

    public List<QuestionBase> Questions {
        get { return questions;}
    }

    public int QuestionCount {
        get { return questions.Count; }
    }
}