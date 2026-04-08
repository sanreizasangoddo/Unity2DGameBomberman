using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;

    [SerializeField] private Sound[] _musicSounds, sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Main Menu Theme");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(_musicSounds, x => x.Name == name);

        if (sound == null)
        {
            Debug.Log("Music not Found");
        } else
        {
            _musicSource.clip = sound.Clip;
            _musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.Name == name);

        if (sound == null)
        {
            Debug.Log("SFX not Found");
        } else
        {
            _sfxSource.PlayOneShot(sound.Clip);
        }
    }
}
