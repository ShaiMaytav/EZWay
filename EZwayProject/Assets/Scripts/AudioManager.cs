using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
}
