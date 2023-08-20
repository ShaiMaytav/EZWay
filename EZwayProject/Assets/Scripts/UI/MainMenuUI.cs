using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : BaseUIElement
{
    private void OnEnable()
    {
        UIManager.Instance.ChangeBackgroundColors(GameManager.Instance.Data.MainTheme);
    }
}
