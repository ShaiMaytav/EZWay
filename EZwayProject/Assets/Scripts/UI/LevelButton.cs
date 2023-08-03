using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text completedQuestionsText;
    [SerializeField] private Image lockedStatusIcon;
    [SerializeField] private Sprite lockedIcon;
    [SerializeField] private Sprite unlockedIcon;
    [SerializeField] private Button button;

    private int _levelNum;

    public void UpdateInfo(LevelData level)
    {
        _levelNum = level.LevelNum;
        levelText.text = "Level " + _levelNum;
        completedQuestionsText.text = level.CompletedQuestionsCount + "\\" + level.Questions.Count;
        lockedStatusIcon.overrideSprite = level.isUnlocked ? unlockedIcon : lockedIcon;
        button.onClick.AddListener(StartLevel);
    }

    private void StartLevel()
    {
        LiveGameController.Instance.StartLevel(_levelNum - 1);
    }

}
