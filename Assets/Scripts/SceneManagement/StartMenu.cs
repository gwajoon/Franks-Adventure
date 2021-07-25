using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public AudioSource SelectionSound;

    public void PlayGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
        SavingSystem.i.Load("SaveSlot1");
    }

    public void PlaySound()
    {
        SelectionSound.Play();
    }
}
