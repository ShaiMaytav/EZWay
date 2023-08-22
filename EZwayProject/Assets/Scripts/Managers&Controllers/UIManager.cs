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

    [Header("Translucent data")]
    [SerializeField] private GameObject TranslucentImage;

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
        tutorialUI.caller = mainMenuUI.gameObject;
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

    public void LevelsToTutorial()
    {
        levelSelectionUI.gameObject.SetActive(false);
        tutorialUI.gameObject.SetActive (true);
        tutorialUI.caller = levelSelectionUI.gameObject;
    }

    #endregion

    #region GameplayUI Methods
    public void GameToLevelSelection()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        gameplayUI.LevelCompleteWindow.gameObject.SetActive(false);
        gameplayUI.QuestionCompleteWindow.SetActive(false);
        gameplayUI.gameObject.SetActive(false);

        levelSelectionUI.gameObject.SetActive(true);
    }

    public void LevelComplete(bool isLastLevel, bool isFirstTime)
    {
        AudioManager._instance.PlaySFX(Sounds.LevelComplete);

        TranslucentImage.SetActive(true);
        gameplayUI.LevelCompleteWindow.gameObject.SetActive(true);
        gameplayUI.NextLevelButton.SetActive(!isLastLevel);

        gameplayUI.LevelCompleteWindow.PointsIcon.SetActive(isFirstTime);
        gameplayUI.LevelCompleteWindow.PointsTxt.gameObject.SetActive(isFirstTime);


        //sets texts position
        Vector2 newTextPos = gameplayUI.LevelCompleteWindow.Texts.anchoredPosition;
        if (isFirstTime)
        {
            newTextPos.y = gameplayUI.LevelCompleteWindow.HighPosY;
        }
        else
        {
            newTextPos.y = gameplayUI.LevelCompleteWindow.LowPosY;
        }
        gameplayUI.LevelCompleteWindow.Texts.anchoredPosition = newTextPos;

    }

    public void WrongAnswer()
    {
        gameplayUI.WrongAnswerWindow.SetActive(true);
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
        gameplayUI.HintButton.SetStatusSprite();
    }

    public void UpdateQuestionsTrack(string _text)
    {
        gameplayUI.QuestTrackTxt.text = _text;
    }

    public void UpdateLevelCompletionWindow(int reward, int levelNum)
    {
        gameplayUI.LevelCompleteWindow.LevelText.text = _gameManager.Data.LevelCompletionText[levelNum];
        gameplayUI.LevelCompleteWindow.EncourageTxt.text = _gameManager.Data.GetRandomEncouragement();
        gameplayUI.LevelCompleteWindow.PointsTxt.text = reward.ToString();
    }

    public void EnableNoPointsMenu()
    {
        AudioManager._instance.PlaySFX(Sounds.Negative);

        TranslucentImage.SetActive(true);

        gameplayUI.NotEnoughPointsWindow.SetActive(true);
    }

    public void EnableOfferMenu()
    {
        TranslucentImage.SetActive(true);

        gameplayUI.OfferWindow.SetActive(true);
    }

    public void UpdateLevelIcon()
    {
        //dynamic
        gameplayUI.TitleImage.sprite = _gameManager.Data.GetTitleIcon(_gameManager.TitleRank);
    }

    public void UpdateLevelIcon(int index)
    {
        //static
        gameplayUI.TitleImage.sprite = _gameManager.Data.GetTitleIcon(index);
    }

    public void ChangeUITheme(UITheme theme)
    {
        //change background color
        ChangeBackgroundColors(theme);

        //change fonts color
        foreach (var text in gameplayUI.AllGemaplayTexts)
        {
            text.color = theme.GeneralFontColor;
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

    public void UpdateHintSprite()
    {
        gameplayUI.HintButton.SetStatusSprite();
    }

    #endregion

    #region Tutorial Methods

    public void ChangeTutorialPage(int step)
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

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
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        tutorialUI.gameObject.SetActive(false);
        tutorialUI.NextButton.gameObject.SetActive(true);

        tutorialUI.caller.SetActive(true);
    }

    #endregion

    #region Other Methods

    public void ToggleOptions()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

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
