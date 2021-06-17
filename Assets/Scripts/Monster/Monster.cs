using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Monster 
{
    [SerializeField] MonsterBase _base;
    [SerializeField] int level;
    public MonsterBase Base
    {
        get {
            return _base;
        }
    }

    public int Level
    {
        get {
            return level;
        }
    }

    public int HP { get; set; }

    public void Init()
    {
        HP = Base.MaxHp;
    }

    public int MaxHp {
        get { return Base.MaxHp; }
    }

    public int Attack {
        get { return Mathf.FloorToInt(Base.Attack);}
    }

    public bool TakeDamage(string answer, Monster attacker)
    {
        int damage = Mathf.FloorToInt(attacker.Attack);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }
        return false;
    }
}
