using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/Data", order = 2)]
public class DataSO : ScriptableObject
{
    public int HintCost;
    public int QuestionReward;
    public int LevelReward;
    public List<string> Encouragements;
    public List<string> QuestionCompletionText;
    public List<string> LevelCompletionText;
    public UITheme MainTheme;
    public List<UITheme> Themes;
    public List<Sprite> TitleSprites;
    public List<Sprite> LevelButtonSprites;

    public string GetRandomEncouragement()
    {
        return Encouragements[Random.Range(0, Encouragements.Count)];
    }

    public string GetRandomQuestText()
    {
        return QuestionCompletionText[Random.Range(0, QuestionCompletionText.Count)];
    }

    public string GetQuestText(int index)
    {
        int newIndex = index % QuestionCompletionText.Count;
        return QuestionCompletionText[newIndex];
    }

    public Sprite GetTitleIcon(int rank)
    {
        return TitleSprites[rank];
    }
   
}

