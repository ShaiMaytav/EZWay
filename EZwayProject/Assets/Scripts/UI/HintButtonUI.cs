using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButtonUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> statusSprites;  

    public void SetStatusSprite()
    {
        GameManager _gameManager = GameManager.Instance;
        image.sprite = _gameManager.Points >= _gameManager.Data.HintCost ? statusSprites[1] : statusSprites[0];
    }
}
