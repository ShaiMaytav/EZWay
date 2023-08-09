using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPool : MonoBehaviour
{
    public List<LetterSlot> AllSlots;

    [SerializeField] private int maxLetterQuantity = 2;

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

        //string letters = "???????????????????????????";
        string letters = "\u05D0\u05D1\u05D2\u05D3\u05D4\u05D5\u05D6\u05D7\u05D8\u05D9\u05DB\u05DC\u05DE\u05DF\u05E0\u05E1\u05E2\u05E4\u05E5\u05E6\u05E7\u05E8\u05E9\u05EA";

        for (int i = 0; i < maxLetterQuantity - 1; i++)
        {
            letters += letters;
        }

        List<char> lettersList = new List<char>(letters);

        //places answer letters randomly in pool
        foreach (var letter in LiveGameController.Instance.CurrentQuestion.Answer)
        {
            tmpSlot = _slots[Random.Range(0, _slots.Count)];
            tmpSlot.SetLetter(letter.ToString());
            _slots.Remove(tmpSlot);
        }

        //fills rest of pool with random letters
        for (int i = _slots.Count; i > 0; i--)
        {
            tmpSlot = _slots[Random.Range(0, i)];
            int _tmpLetterIndex = Random.Range(0, lettersList.Count);
            tmpSlot.SetLetter((lettersList[_tmpLetterIndex]).ToString());
            lettersList.RemoveAt(_tmpLetterIndex);
            _slots.Remove(tmpSlot);
        }
    }

}
