using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int LevelNum ;
    public bool isUnlocked;
    public List<QuestionData> Questions;
    [HideInInspector]public int CompletedQuestionsCount = 0;
    public int QuestionsAmount { get { return Questions.Count; }}
}
