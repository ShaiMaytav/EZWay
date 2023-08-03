using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameplayUI gameplayUI;

    private GameManager _gameManager;
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("A UIManager component was removed from " + gameObject.name);
            Destroy(this);
        }

        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        levelSelectionUI.CreateLevelsButtons();
    }

    public void Play()
    {
        mainMenuUI.gameObject.SetActive(false);
        levelSelectionUI.gameObject.SetActive(true);
    }

    public void StartLevel()
    {
        levelSelectionUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }

    public void GameToLevelSelection()
    {
        gameplayUI.LevelCompleteWindow.SetActive(false);
        gameplayUI.QuestionCompleteWindow.SetActive(false);
        gameplayUI.gameObject.SetActive(false);

        levelSelectionUI.gameObject.SetActive(true);
    }

    public void LevelComplete()
    {
        gameplayUI.LevelCompleteWindow.SetActive(true);
    }

    public void QuestionComplete()
    {
        gameplayUI.QuestionCompleteWindow.SetActive(true);
    }
}
