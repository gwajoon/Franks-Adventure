using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal; //identifies the destination portal
    [SerializeField] Transform spawnPoint;


    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }
    
    Fader fader;
    private void Start()
    {
      fader = FindObjectOfType<Fader>();  
    }
    

    IEnumerator SwitchScene()
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);
        
        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier { HomeTownDesert, DesertCave, CaveSnow, CaveRiddleA, CaveRiddleB, SnowHomeTown, HomeTownHouseA, HomeTownHouseB, 
DesertHouseA, DesertHouseB, DesertHouseC, SnowHouseA, SnowHouseB }
