using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [SerializeField] private List<Sprite> onOffSprites;
    [SerializeField] private Image image;
    [SerializeField] private Button button;


    private void Start()
    {
        button.onClick.AddListener(Toggle);
    }

    private void OnEnable()
    {
        UpdateSprite();
    }

    private void Toggle()
    {
        AudioManager._instance.ToggleGroupVolume("SFXVolume");
        UpdateSprite();
    }
    
    private void UpdateSprite()
    {
        float curVol = AudioManager._instance.GetGroupVolume("SFXVolume");

        image.sprite = curVol == -80 ? onOffSprites[0] : onOffSprites[1];
    }
}
