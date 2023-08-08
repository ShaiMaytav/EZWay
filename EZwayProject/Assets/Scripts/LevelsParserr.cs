using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsParserr : MonoBehaviour
{
    [SerializeField] private SheetReader reader;

    public List<LevelData> GetLevelsFromSheets()
    {
        List<LevelData> levels = new List<LevelData>();
        List<object> sheetQuests = (List<object>)reader.getSheetRange();





        return levels;
    }

    private QuestionData ParseQuestion()
    {
        return new QuestionData();
    }
}
