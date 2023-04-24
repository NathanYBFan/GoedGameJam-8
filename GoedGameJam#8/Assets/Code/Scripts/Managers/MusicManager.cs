using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] music;
    public AudioSource musicPlayer;

    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapMusic(AudioClip musicToPlay)
    {
        if (musicPlayer.clip != null)
            musicPlayer.Stop();
        musicPlayer.clip = musicToPlay;            
        musicPlayer.Play();
    }
}
