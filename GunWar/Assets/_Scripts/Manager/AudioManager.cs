using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Audio[] audios;
    void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        foreach (Audio audio in audios)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;
            audio.source.volume = audio.volume;
            audio.source.pitch = audio.pitch;
            audio.source.loop = audio.loop;
        }
    }

    public void Play(string name)
    {
        Audio audio = Array.Find(audios, sound => sound.name == name);
        if (audio == null)
        {
            Debug.Log("Can't find audio!");
            return;
        }
        if (audio.audioType == AudioType.Sound)
        {
            if (Utility.onSound == 1) return;
        }
        if (audio.audioType == AudioType.Music)
        {
            if (Utility.onMusic == 1) return;
        }
        audio.source.Play();
    }
    public void Stop(string name)
    {
        Audio audio = Array.Find(audios, sound => sound.name == name);
        if (audio == null)
        {
            Debug.Log("Can't find audio!");
            return;
        }
        audio.source.Stop();
    }
}

[System.Serializable]
public class Audio
{
    public string name;
    public AudioType audioType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1;
    [Range(.1f, 3f)] public float pitch = 1;
    public bool loop;
    [HideInInspector] public AudioSource source;
}

public enum AudioType
{
    Sound,
    Music
}