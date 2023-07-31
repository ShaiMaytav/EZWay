using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveGameController : MonoBehaviour
{
    public List<LetterSlot> letterPool;
    public List<LetterSlot> answerSlot;
    public QuestionData currentQuestion;
    public LevelData currentLevel;

    public static LiveGameController Instance { get { return _instance; } }
    private static LiveGameController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("A LiveGameComponent component was removed from " + gameObject.name);
            Destroy(this);
        }
    }
}
