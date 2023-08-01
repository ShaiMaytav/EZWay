using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    public string LetterValue;
    [SerializeField] private TMP_Text letterTxt;

    public void UpdateLetter(string _letter)
    {
        LetterValue = _letter;
        letterTxt.text = LetterValue;
    }
}
