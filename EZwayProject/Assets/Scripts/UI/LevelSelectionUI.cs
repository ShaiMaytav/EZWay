using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private RectTransform levelLayout;


    private List<LevelButton> _levelsButtons = new List<LevelButton>();
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        UpdateButtonsInfo();
    }

    public void CreateLevelsButtons()
    {
        foreach (var level in GameManager.Instance.Levels)
        {
            _levelsButtons.Add(Instantiate(GameManager.Instance.Prefabs.LevelButtonPrefab, levelLayout));
        }
    }

    public void UpdateButtonsInfo()
    {
        for (int i = 0; i < _levelsButtons.Count; i++)
        {
            _levelsButtons[i].UpdateInfo(_gameManager.Levels[i]);
        }
    }
}
