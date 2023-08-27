using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : BaseUIElement
{
    public Image PointsBackgroundImage;
    public Image BurgerImage;
    public Image TitleImage;
    public QuestionCompleteWindow QuestionCompleteWindow;
    public GameObject WrongAnswerWindow;
    public GameObject NotEnoughPointsWindow;
    public OfferWindowUI OfferWindow;
    public LevelCompleteUI LevelCompleteWindow;
    public GameObject NextLevelButton;
    public TMP_Text QuestExampleTxt;
    public TMP_Text QuestConditionTxt;
    public TMP_Text QuestQuestionTxt;
    public TMP_Text PointsTxt;
    public TMP_Text QuestTrackTxt;
    public HintButtonUI HintButton;
    public List<TMP_Text> AllGemaplayTexts;
}
