using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlot : MonoBehaviour
{
    public LetterObject CurrentLetter;

    public bool IsEmpty
    {
        get
        { return !CurrentLetter; }
    }

    private LiveGameController _gameController;
    private GameManager _gameManager;

    private void Start()
    {
        _gameController = LiveGameController.Instance;
        _gameManager = GameManager.Instance;    
    }

    public void Pick()
    {
        _gameController.PickLetter(this);
    }

    public void CreateLetter()
    {
        CurrentLetter = Instantiate(GameManager.Instance.Prefabs.LetterObjectPrefab, transform.position, transform.rotation, transform);
    }

    public void SetLetter(string value)
    {
        if (CurrentLetter)
        {
            CurrentLetter.UpdateLetter(value);
        }
    }

    public void SendLetterToSlot(LetterSlot slot)
    {
        CurrentLetter.ChangeSlot(slot);
        CurrentLetter = null;
    }
}
