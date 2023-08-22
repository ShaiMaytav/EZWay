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
    public List<string> LevelCompletionText;
    public UITheme MainTheme;
    public List<UITheme> Themes;
    public List<Sprite> TitleSprites;
    public List<Sprite> LevelButtonSprites;

    public string GetRandomEncouragement()
    {
        return Encouragements[Random.Range(0, Encouragements.Count)];
    }

    public Sprite GetTitleIcon(int rank)
    {
        return TitleSprites[rank];
    }
   
}

