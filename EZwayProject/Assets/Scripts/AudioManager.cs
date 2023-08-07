using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private List<Sound> sounds;

    public AudioManager Instance { get { return _instance;} private set { } }
    private AudioManager _instance;

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

    public void PlaySound(Sounds soundType)
    {
        foreach (var sound in sounds)
        {
            if (sound.type == soundType)
            {
                sound.audioSource.Play();
                break;
            }
        }
    }

    public void PlaySound(string soundName)
    {
        foreach (var sound in sounds)
        {
            if (sound.type.ToString() == soundName)
            {
                sound.audioSource.Play();
                break;
            }
        }
    }
}
