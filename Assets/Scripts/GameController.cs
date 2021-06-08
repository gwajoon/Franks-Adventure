using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// States of game displayed on the screen
// FreeRoam is when we control Frank on the map, Battle is when Frank engages in battle
public enum GameState { FreeRoam, Battle, Dialogue , Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    GameState stateBeforePause;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;

        DialogueManager.Instance.OnShowDialogue += () => 
        {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.onCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
                state = GameState.FreeRoam;
        };
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            stateBeforePause = state;
            state = GameState.Paused;
        }
        else
        {
            state = stateBeforePause;
        }
    }

    public void StartBattle()
    {
        state = GameState.Battle;

        // set battle system to be active
        battleSystem.gameObject.SetActive(true);

        //disable main camera
        worldCamera.gameObject.SetActive(false);

        Monster Frank = playerController.getMonster;
        Monster wildMonster = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildMonster();

        battleSystem.StartBattle(Frank, wildMonster);
    }

    TrainerController trainer;
    
    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        Monster Frank = playerController.getMonster;
        battleSystem.StartTrainerBattle(Frank, trainer);
    }

    public void OnEnterTrainerView(TrainerController trainer)
    {
        state = GameState.Dialogue;
        StartCoroutine(trainer.TriggerTrainerBattle(playerController));
    }

    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }

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
            //battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
