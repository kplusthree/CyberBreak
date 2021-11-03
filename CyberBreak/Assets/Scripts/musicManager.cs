using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    public AudioClip mainMenuMusicClip;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = mainMenuMusicClip;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
