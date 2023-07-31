using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlot : MonoBehaviour
{
    public bool isEmpty = true;
    public Letter letter;
    
    private LiveGameController _gameController;

    private void Start()
    {
        _gameController = LiveGameController.Instance;
    }
}
