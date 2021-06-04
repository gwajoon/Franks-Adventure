using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void onPlayerTriggered(PlayerController player)
    {
        // switch scene  
        Debug.Log("Player has entered the portal");
    }
}