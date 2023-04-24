using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicConfiguration : MonoBehaviour
{
    public AudioClip musicToPlay;
    public MusicManager musicPlayer;
    void Awake()
    {
        musicPlayer.SwapMusic(musicToPlay);
    }
}
