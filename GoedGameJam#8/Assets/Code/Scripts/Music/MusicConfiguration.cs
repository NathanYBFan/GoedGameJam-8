using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicConfiguration : MonoBehaviour
{
    public AudioClip musicToPlay;
    public MusicManager musicPlayer;
    void Awake()
    {
        musicPlayer = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        musicPlayer.SwapMusic(musicToPlay);
    }
}
