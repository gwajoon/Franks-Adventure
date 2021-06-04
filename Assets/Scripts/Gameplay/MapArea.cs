using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all the wild monsters in an area
public class MapArea : MonoBehaviour
{
    [SerializeField] List<Monster> wildMonsters;

    public Monster GetRandomWildMonster()
    {
        var wildMonster = wildMonsters[Random.Range(0, wildMonsters.Count)];
        wildMonster.Init();
        return wildMonster;
    }
}
