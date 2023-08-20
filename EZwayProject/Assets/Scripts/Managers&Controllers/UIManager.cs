using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameplayUI gameplayUI;
    [SerializeField] private GameObject noConnectionUI;
    [SerializeField] private GameObject optionsWindowsUI;
    [SerializeField] private Image background;

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
        Invoke("Init", 0.1f);
    }

    private void Init()
    {
        levelSelectionUI.CreateLevelsButtons();
    }

    #region MainMenu Methods
    public void Play()
    {
        mainMenuUI.gameObject.SetActive(false);
        levelSelectionUI.gameObject.SetActive(true);
    }

    public void MainMenuToTutorial()
    {
        mainMenuUI.gameObject.SetActive(false);
        tutorialUI.gameObject.SetActive(true);
    }

    public void Startup(bool isConnected)
    {
        if (isConnected)
        {
            mainMenuUI.gameObject.SetActive(true);
        }
        else
        {
            noConnectionUI.SetActive(true);
        }
    }
    #endregion

    #region LevelSelection Methods

    public void StartLevel()
    {
        levelSelectionUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }

    public void LevelsToMainMenu()
    {
        levelSelectionUI.gameObject.SetActive(false);
        mainMenuUI.gameObject.SetActive(true);
    }
    #endregion

    #region GameplayUI Methods
    public void GameToLevelSelection()
    {
        gameplayUI.LevelCompleteWindow.gameObject.SetActive(false);
        gameplayUI.QuestionCompleteWindow.SetActive(false);
        gameplayUI.gameObject.SetActive(false);

        levelSelectionUI.gameObject.SetActive(true);
    }

    public void LevelComplete(bool isLastLevel, bool isFirstTime)
    {
        gameplayUI.LevelCompleteWindow.gameObject.SetActive(true);
        gameplayUI.NextLevelButton.SetActive(!isLastLevel);

        gameplayUI.LevelCompleteWindow.PointsIcon.SetActive(isFirstTime);
        gameplayUI.LevelCompleteWindow.PointsTxt.gameObject.SetActive(isFirstTime);
    }

    public void QuestionComplete()
    {
        gameplayUI.QuestionCompleteWindow.SetActive(true);
    }

    public void NextLevel()
    {
        gameplayUI.LevelCompleteWindow.gameObject.SetActive(false);
    }

    public void UpdateQuestionText(string example, string condition, string question)
    {
        gameplayUI.QuestExampleTxt.text = example;
        gameplayUI.QuestConditionTxt.text = condition;
        gameplayUI.QuestQuestionTxt.text = question;
    }

    public void UpdatePointsText()
    {
        gameplayUI.PointsTxt.text = _gameManager.Points.ToString();
    }

    public void UpdateQuestionsTrack(string _text)
    {
        gameplayUI.QuestTrackTxt.text = _text;
    }

    public void UpdateLevelCompletionWindow(int reward)
    {
        gameplayUI.LevelCompleteWindow.EncourageTxt.text = _gameManager.Data.GetRandomEncouragement();
        gameplayUI.LevelCompleteWindow.PointsTxt.text = reward.ToString();
    }

    public void EnableNoPointsMenu()
    {
        gameplayUI.NotEnoughPointsWindow.SetActive(true);
    }

    public void EnableOfferMenu()
    {
        gameplayUI.OfferWindow.SetActive(true);
    }

    public void UpdateLevelIcon()
    {
        gameplayUI.TitleImage.sprite = _gameManager.Data.GetTitleIcon(_gameManager.TitleRank);
    }

    public void ChangeUITheme(UITheme theme)
    {
        //change background color
        ChangeBackgroundColors(theme);

        //change fonts color
        foreach (var text in gameplayUI.AllGemaplayTexts)
        {
            text.color = theme.FontColor;
        }
        
        gameplayUI.BurgerImage.sprite = theme.BurgerImage;
        gameplayUI.PointsBackgroundImage.sprite = theme.PointsBackgroundImage;
    }

    public void ChangeBackgroundColors(UITheme theme)
    {
        background.material.SetColor("_TopColor", theme.TopColor);
        background.material.SetColor("_BottomColor", theme.BottomColor);
        background.material.SetFloat("_GradientSpread", theme.GradiantSpread);
    }

    #endregion

    #region Tutorial Methods

    public void ChangeTutorialPage(int step)
    {

        tutorialUI.Screens[tutorialUI.ScreenIndex].SetActive(false);
        if (!tutorialUI.PreviousButton.activeSelf)
        {
            tutorialUI.PreviousButton.SetActive(true);
        }
        else if (!tutorialUI.NextButton.activeSelf)
        {
            tutorialUI.NextButton.SetActive(true);

        }


        if (tutorialUI.ScreenIndex + step <= 0)
        {
            tutorialUI.PreviousButton.SetActive(false);
        }
        else if (tutorialUI.ScreenIndex + step >= tutorialUI.Screens.Count - 1)
        {
            tutorialUI.NextButton.SetActive(false);
        }

        tutorialUI.ScreenIndex += step;
        tutorialUI.Screens[tutorialUI.ScreenIndex].SetActive(true);

    }

    public void TutorialToMainMenu()
    {
        tutorialUI.gameObject.SetActive(false);
        tutorialUI.NextButton.gameObject.SetActive(true);
        mainMenuUI.gameObject.SetActive(true);
    }

    #endregion

    #region Other Methods

    public void ToggleOptions()
    {
        optionsWindowsUI.SetActive(!optionsWindowsUI.activeSelf);
    }

    public void OptionsToMenu()
    {
        ToggleOptions();

        if (gameplayUI.gameObject.activeSelf)
        {
            gameplayUI.gameObject.SetActive(false);
        }
        else if (levelSelectionUI.gameObject.activeSelf)
        {
            levelSelectionUI.gameObject.SetActive(false);
        }

        mainMenuUI.gameObject.SetActive(true);
    }

    #endregion
}