using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource AudioSource;
    private float musicVolume = 0.25f; 
    void Start()
    {
        AudioSource.Play();
    }

    void Update()
    {
        AudioSource.volume = musicVolume;
    }
 
    public void updateVolume( float volume )
    {
        musicVolume = volume;
    }
}
