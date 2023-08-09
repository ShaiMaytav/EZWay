using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsParser : MonoBehaviour
{
    [SerializeField] private SheetReader reader;

    public List<LevelData> GetLevelsFromSheets()
    {
        List<LevelData> _levels = new List<LevelData>();
        IList<IList<object>> _sheetQuests = reader.getSheetRange();

        LevelData _currentLevel = new LevelData(1);
        _levels.Add(_currentLevel);

        foreach (var question in _sheetQuests)
        {
            QuestionData newQuestion = ParseQuestion(question);
            if (((string)question[0]) != _currentLevel.LevelNum.ToString())
            {
                _currentLevel = new LevelData(int.Parse((string)question[0]));
                _levels.Add(_currentLevel);
            }
            _currentLevel.Questions.Add(newQuestion);
        }


        return _levels;
    }

    private QuestionData ParseQuestion(IList<object> qusetion)
    {
        QuestionData _questionData = new QuestionData();
        _questionData.Example = (string)qusetion[1];
        _questionData.Condition = (string)qusetion[2];
        _questionData.Question = (string)qusetion[3];
        _questionData.Answer = (string)qusetion[4];
        return _questionData;
    }
}
