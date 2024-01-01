using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string title;
    public AudioClip soundClip;
    public float volume;
    public bool isLooping;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] soundsInGame;
    private Dictionary<string, AudioSource> soundDict = new Dictionary<string, AudioSource>();
    private AudioSource ownAudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < soundsInGame.Length; i++)
        {
            AudioSource currentSource = gameObject.AddComponent<AudioSource>();
            currentSource.loop = soundsInGame[i].isLooping;
            currentSource.clip = soundsInGame[i].soundClip;
            currentSource.volume = soundsInGame[i].volume;
            soundDict.Add(soundsInGame[i].title, currentSource);
        }
    }

    public void PlaySound(string soundTitle)
    {
        if (!soundDict.ContainsKey(soundTitle))
        {
            Debug.Log("sound title doesnt exist");
            return;
        }
        soundDict[soundTitle].Play();
    }
    
    public void StopSound(string soundTitle) //for looping sounds
    {
        if (!soundDict.ContainsKey(soundTitle))
        {
            return;
        }
        soundDict[soundTitle].Stop();
    }
}
