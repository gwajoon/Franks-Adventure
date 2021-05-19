using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// States of game displayed on the screen
// FreeRoam is when we control Frank on the map, Battle is when Frank engages in battle
public enum GameState { FreeRoam, Battle }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        //battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        state = GameState.Battle;

        // set battle system to be active
        battleSystem.gameObject.SetActive(true);

        //disable main camera
        worldCamera.gameObject.SetActive(false);

        battleSystem.Start();
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
    }
}
