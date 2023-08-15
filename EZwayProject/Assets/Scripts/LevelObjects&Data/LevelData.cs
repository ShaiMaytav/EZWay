using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData 
{
    public int LevelNum;
    public bool IsUnlocked;
    public int RewardPoints;
    public List<QuestionData> Questions = new List<QuestionData>();
    [HideInInspector] public int CompletedQuestionsCount = 0;
    public int QuestionsAmount { get { return Questions.Count; } }
    public bool IsCompleted { get { return CompletedQuestionsCount >= QuestionsAmount; } }

    public LevelData(int _levelNum)
    {
        LevelNum = _levelNum;
    }
    public LevelData(){}
}
