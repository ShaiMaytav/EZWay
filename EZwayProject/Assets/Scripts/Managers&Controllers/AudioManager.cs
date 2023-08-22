using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Sounds
{
    ExtractLetter,
    InsertLetter,
    LevelComplete,
    Negative,
    QuestionComplete,
    UIClick
}

public class AudioManager : MonoBehaviour
{

    //[System.Serializable]
    //public class Sound
    //{
    //    public Sounds type;
    //    public AudioSource audioSource;
    //}

    [SerializeField] private AudioMixer mainMixer;
    [Range(0, 100)]
    [SerializeField] private float mainVolume = 0;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [Range(0, 100)]
    [SerializeField] private float sfxVolume = 0;
    [SerializeField] private List<AudioSource> sfxSources;

    Dictionary<Sounds, AudioSource> sfxSoundsDict;
    public static AudioManager _instance;

    private void OnValidate()
    {
        foreach (var sound in sfxSources)
        {
            sound.outputAudioMixerGroup = sfxMixer;
        }

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("An AudioManager component was removed from " + gameObject.name);
            Destroy(this);
        }

    }

    private void Start()
    {
        sfxSoundsDict = new Dictionary<Sounds, AudioSource>();


        for (int i = 0; i < Enum.GetValues(typeof(Sounds)).Length; i++)
        {
            sfxSoundsDict.Add((Sounds)i, sfxSources[i]);
        }

        // -80 because of minimum decibles of unity audio mixers
        mainMixer.SetFloat("MasterVolume", mainVolume - 80);
        mainMixer.SetFloat("SFXVolume", sfxVolume - 80);
    }

    public void PlaySFX(Sounds soundType)
    {
        sfxSoundsDict[soundType].Play();
    }
    public void PlaySFX(int soundIndex)
    {
        sfxSoundsDict[(Sounds)soundIndex].Play();
    }

    public float GetGroupVolume(string groupName)
    {
        float resVol = 0;
        switch (groupName)
        {
            case "SFXVolume":
                mainMixer.GetFloat(groupName, out resVol);
                break;
            case "MasterVolume":
                mainMixer.GetFloat(groupName, out resVol);
                break;
            default:
                break;
        }
        return resVol;
    }

    public void ToggleGroupVolume(string groupVolName)
    {
        float curVolume;
        mainMixer.GetFloat(groupVolName, out curVolume);

        switch (groupVolName)
        {
            case "SFXVolume":
                mainMixer.SetFloat(groupVolName, curVolume == -80 ? sfxVolume - 80 : -80);
                
                if(curVolume == -80)
                {
                    PlaySFX(Sounds.UIClick);
                }


                break;
            case "MasterVolume":
                mainMixer.SetFloat(groupVolName, curVolume == -80 ? mainVolume - 80 : -80);

                if (curVolume == -80)
                {
                    PlaySFX(Sounds.UIClick);
                }
                break;
            default:
                break;
        }
    }
}
