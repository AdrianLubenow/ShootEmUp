using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;


            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = sfxMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Master"))
        {
            PlayerPrefs.SetFloat("Master", 0);
            Load();
        }
        else
        {
            Load();
        }

        if (!PlayerPrefs.HasKey("SoundFX"))
        {
            PlayerPrefs.SetFloat("SoundFX", 0);
            Load();
        }
        else
        {
            Load();
        }

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 0);
            Load();
        }
        else
        {
            Load();
        }

        Play("MainMenuThemeTrap");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }


    private void Load()
    {
        UIManager.instance.masterSlider.value = PlayerPrefs.GetFloat("Master");
        UIManager.instance.sfxSlider.value = PlayerPrefs.GetFloat("SoundFX");
        UIManager.instance.musicSlider.value = PlayerPrefs.GetFloat("Music");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Master", UIManager.instance.masterSlider.value);
        PlayerPrefs.SetFloat("SoundFX", UIManager.instance.sfxSlider.value);
        PlayerPrefs.SetFloat("Music", UIManager.instance.musicSlider.value);
    }
}
