using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] Sounds[] sounds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static SFXManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
            return;

        }
        DontDestroyOnLoad(gameObject);

        foreach (Sounds s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip=s.audioClip;
            s.source.volume=s.volume;
            s.source.pitch=s.pitch;
        }
    }       

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds,sound=>sound.clipName==name);
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds,sound=>sound.clipName==name);
        s.source.Stop();
    }
}
