using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]

public class MonsterBase : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    // randomise this in the future when there are more questions
    [SerializeField] List<QuestionBase> questions;

    [SerializeField] Sprite sprite;

    [SerializeField] MonsterType type1;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;

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

    public int Defense {
        get { return defense; }
    }

    public int Speed {
        get { return speed; }
    }

    public Sprite Sprite {
        get { return sprite; }
    }

    public QuestionBase Question(int index) {
        return questions[index];
    }

    public string QuestionName(int index) {
        return questions[index].name;
    }

    public int QuestionCount {
        get { return questions.Count; }
    }
}

public enum MonsterType
{
    None,
    Recursion,
    ForLoop,
    WhileLoop
}
