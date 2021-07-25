using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Monster _monster;

    public void setData(Monster monster)
    {
        _monster = monster;

        nameText.text = monster.Base.Name;

        if (monster.Base.name == "Frank")
        {
            monster.HP = 100;
        }
        hpBar.setHP((float) monster.HP / monster.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.setHPSmooth((float)_monster.HP / _monster.MaxHp);
    }
}
