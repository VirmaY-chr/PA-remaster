using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource sfx;

    public static AudioManager inst;
    void Awake()
    {
        inst = this;
        source = GameObject.Find("AudioSource").GetComponents<AudioSource>()[0];
        sfx = GameObject.Find("AudioSource").GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
