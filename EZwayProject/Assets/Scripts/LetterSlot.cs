using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlot : MonoBehaviour
{
    public bool IsEmpty = true;
    public Letter LetterPrefab;
    public Letter CurrentLetter;
    
    private LiveGameController _gameController;

    private void Start()
    {
        _gameController = LiveGameController.Instance;
    }

    public void Pick()
    {
        print("pick");
    }

    public void CreateLetter()
    {
        CurrentLetter = Instantiate(LetterPrefab, transform.position, transform.rotation, transform);
        IsEmpty = false;
    }

    public void SetLetter(string value)
    {
        if (CurrentLetter)
        {
            CurrentLetter.UpdateLetter(value);
        }
    }
}
