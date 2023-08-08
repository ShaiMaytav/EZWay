using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum Sounds
    {
        Click,
        Toggle,
        LevelComplete,
        QuestAnswered
    }

    [System.Serializable]
    public class Sound
    {
        public Sounds type;
        public AudioSource audioSource;
    }

    [SerializeField] private AudioMixer mainMixer;
    [Range(0, 100)]
    [SerializeField] private float mainVolume = 0;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [Range(0, 100)]
    [SerializeField] private float sfxVolume = 0;
    [SerializeField] private List<Sound> sfxSounds;

    public AudioManager Instance { get { return _instance; } private set { } }
    private AudioManager _instance;

    private void OnValidate()
    {
        foreach (var sound in sfxSounds)
        {
            sound.audioSource.outputAudioMixerGroup = sfxMixer;
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
        // -80 because of minimum decibles of unity audio mixsers
        mainMixer.SetFloat("MasterVolume", mainVolume - 80);
        mainMixer.SetFloat("SFXVolume", sfxVolume - 80);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaySFX(Sounds.Click);
        }
    }

    public void PlaySFX(Sounds soundType)
    {
        foreach (var sound in sfxSounds)
        {
            if (sound.type == soundType)
            {
                sound.audioSource.Play();
                break;
            }
        }
    }

    public void PlaySFX(string soundName)
    {
        foreach (var sound in sfxSounds)
        {
            if (sound.type.ToString() == soundName)
            {
                sound.audioSource.Play();
                break;
            }
        }
    }

    public void ToggleGroupVolume(string groupVolName)
    {
        float curVolume;
        mainMixer.GetFloat(groupVolName, out curVolume);

        switch (groupVolName)
        {
            case "SFXVolume":
                mainMixer.SetFloat(groupVolName, curVolume == -80 ? sfxVolume - 80 : -80);
                break;
            case "MasterVolume":
                mainMixer.SetFloat(groupVolName, curVolume == -80 ? mainVolume - 80 : -80);
                break;
            default:
                break;
        }
    }
}
