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

    private LevelData _level;

    public void UpdateInfo(LevelData level)
    {
        _level = level;
        levelText.text = "Level " + _level.LevelNum;
        completedQuestionsText.text = level.CompletedQuestionsCount + "\\" + level.Questions.Count;
        lockedStatusIcon.overrideSprite = level.isUnlocked ? unlockedIcon : lockedIcon;
        button.onClick.AddListener(StartLevel);
    }

    private void StartLevel()
    {
        if (_level.isUnlocked)
        {
            UIManager.Instance.StartLevel();
            LiveGameController.Instance.StartLevel(_level.LevelNum - 1);
            print("Entering level " + _level.LevelNum);
        }
    }

}
