using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteUI : BaseUIElement
{
    public TMP_Text VictoryText;
    public TMP_Text LevelText;
    public TMP_Text EncourageTxt;
    public TMP_Text PointsTxt;
    public GameObject PointsIcon;
    public RectTransform Texts;

    public float HighPosY;
    public float LowPosY;

    private void OnDisable()
    {
        UIManager.Instance.SetConfettiActive(false);
    }
}
