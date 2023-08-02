using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPool : MonoBehaviour
{
    public List<LetterSlot> AllSlots;
    public bool IsEmpty
    {
        get
        {
            foreach (var slot in AllSlots)
            {
                if (!slot.IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public List<LetterSlot> EmptySlots
    {
        get
        {
            List<LetterSlot> slots = new List<LetterSlot>();
            foreach (var slot in AllSlots)
            {
                if (slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
            return slots;
        }
    }

    private LiveGameController _controller;

    private void Awake()
    {
        _controller = LiveGameController.Instance;

        CreateLetters();
    }

    public void CreateLetters()
    {
        if (IsEmpty)
        {
            foreach (var slot in AllSlots)
            {
                slot.CreateLetter();
            }
        }
    }

    public void SetLetters()
    {
        List<LetterSlot> _slots = new List<LetterSlot>(AllSlots);
        LetterSlot tmpSlot;
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //places answer letters randomly in pool
        foreach (var letter in _controller.CurrentQuestion.Answer)
        {
            tmpSlot = _slots[Random.Range(0, _slots.Count)];
            tmpSlot.SetLetter(letter.ToString());
            _slots.Remove(tmpSlot);
        }

        //fills rest of pool with random letters
        for (int i = _slots.Count; i > 0; i--)
        {
            tmpSlot = _slots[Random.Range(0, i)];
            tmpSlot.SetLetter((letters[Random.Range(0, letters.Length)]).ToString());
            _slots.Remove(tmpSlot);
        }
    }

}
