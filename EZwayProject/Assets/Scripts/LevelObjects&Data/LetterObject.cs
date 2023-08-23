using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterObject : MonoBehaviour
{
    public string LetterValue;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text letterTxt;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private RectTransform rectTransform;

    public void UpdateLetter(string _letter)
    {
        LetterValue = _letter;
        letterTxt.text = LetterValue;
    }

    public void ChangeSlotTween(LetterSlot slot)
    {
        //slot.CurrentLetter = this;
        //transform.position = slot.transform.position;
        //transform.SetParent(slot.transform);

        transform.SetParent(LiveGameController.Instance.inermediateParent.transform);


        slot.CurrentLetter = this;
        Vector3 targetPosition = slot.transform.position;
        LeanTween.move(gameObject, targetPosition, moveSpeed).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => OnEndLetterMove(slot));
    }

    public void ChangeSlot(LetterSlot slot)
    {
        slot.CurrentLetter = this;
        Vector3 targetPosition = slot.transform.position;
        transform.SetParent(slot.transform);
        rectTransform.position = targetPosition;
    }

    private void OnEndLetterMove(LetterSlot slot)
    {
        rectTransform.position = slot.transform.position;
        transform.SetParent(slot.transform);

    }
    public void ChangeColors(UITheme theme) 
    {
        backgroundImage.sprite = theme.ButtonsSprite;
        //letterTxt.color = theme.LettersFontColor;
    }
}
