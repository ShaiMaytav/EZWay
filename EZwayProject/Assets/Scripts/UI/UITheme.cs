using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UITheme 
{
    public Color TopColor;
    public Color BottomColor;
    [Range(0,5)]
    public float GradiantSpread;

    public Sprite ButtonsSprite;
    public Sprite SlotSprite;
    public Sprite PointsBackgroundImage;
    public Sprite BurgerImage;
    public Color GeneralFontColor;
    public Color LettersFontColor;
}
