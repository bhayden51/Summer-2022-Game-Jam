using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public AudioClip myNewSong;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewSong()
    {
        FindObjectOfType<Music>().ChangeSong(myNewSong);
    }

    public void NewSong(AudioClip newSong)
    {
        FindObjectOfType<Music>().ChangeSong(newSong);
    }
}
