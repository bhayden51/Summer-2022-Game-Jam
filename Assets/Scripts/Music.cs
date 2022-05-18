using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static float maxVolume = .4f;
    public float volumeChangeSpeed;
    public AudioSource currentSong;

    private static Music instance;
    public AudioClip nextSong;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {

        if (nextSong == null)
        {
            if (currentSong.volume < maxVolume)
            {
                currentSong.volume += volumeChangeSpeed * Time.deltaTime;
            }
            else
            {
                currentSong.volume = maxVolume;
            }
        }
        else
        {
            if(currentSong.volume > .01f)
            {
                currentSong.volume -= volumeChangeSpeed * Time.deltaTime;
            }
            else
            {
                currentSong.clip = nextSong;
                currentSong.Play();
                nextSong = null;
            }
        }
    }

    public void ChangeSong(AudioClip newSong)
    {
        nextSong = newSong;
    }
}
