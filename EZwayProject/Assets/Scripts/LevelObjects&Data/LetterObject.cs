using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterObject : MonoBehaviour
{
    public string LetterValue;
    [SerializeField] private TMP_Text letterTxt;
    [SerializeField] private float moveSpeed = 1;

    public void UpdateLetter(string _letter)
    {
        LetterValue = _letter;
        letterTxt.text = LetterValue;
    }

    public void ChangeSlot(LetterSlot slot)
    {
        //slot.CurrentLetter = this;
        //transform.position = slot.transform.position;
        //transform.SetParent(slot.transform);

        slot.CurrentLetter = this;
        Vector3 targetPosition = slot.transform.position;
        LeanTween.move(gameObject, targetPosition, moveSpeed).setEase(LeanTweenType.easeOutQuad);
        transform.SetParent(slot.transform);
    }
}
