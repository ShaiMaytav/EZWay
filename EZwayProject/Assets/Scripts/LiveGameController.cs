using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiveGameController : MonoBehaviour
{
    public LetterPool LetterPool;
    public QuestionData CurrentQuestion;
    public LevelData CurrentLevel;
    public RectTransform AnswerSlotsLayout;
    public UnityEvent OnPoolLetterPicked;
    public List<LetterSlot> AnswerSlots;

    [SerializeField] private RectTransform canvas;

    private int _currentQuestionIndex;
    private GameManager _gameManager;

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

    private void Start()
    {
        _gameManager = GameManager.Instance;
        OnPoolLetterPicked.AddListener(AnswerCheck);
    }

    public void StartLevel(int levelNum)
    {
        CurrentLevel = _gameManager.Levels[levelNum];
        _currentQuestionIndex = CurrentLevel.CompletedQuestionsCount == CurrentLevel.Questions.Count ? 0 : CurrentLevel.CompletedQuestionsCount;
        GenerateQuestion();
    }

    public void PickLetter(LetterSlot letterSlot)
    {
        if (letterSlot.CurrentLetter != null)
        {
            //check if slot belongs to pool and calls method accordingly
            if (LetterPool.AllSlots.Contains(letterSlot))
            {
                PoolToAnswer(letterSlot);
                return;
            }

            //check if letter is in answer slots and calls method accordingly
            if (AnswerSlots.Contains(letterSlot))
            {
                AnswerToPool(letterSlot);
            }

        }
    }

    private void PoolToAnswer(LetterSlot letterSlot)
    {
        LetterSlot _tmpSlot = null;

        //gets first empty slot of answer slots
        foreach (var slot in AnswerSlots)
        {
            if (slot.IsEmpty)
            {
                _tmpSlot = slot;
                break;
            }
        }

        if (_tmpSlot == null)
        {
            print("Answer slots are all occupied");
            return;
        }

        letterSlot.SendLetterToSlot(_tmpSlot);
        OnPoolLetterPicked.Invoke();
    }

    private void AnswerToPool(LetterSlot letterSlot)
    {
        LetterSlot _tmpSlot = LetterPool.EmptySlots[Random.Range(0, LetterPool.EmptySlots.Count)];

        if (_tmpSlot == null)
        {
            Debug.LogError("Letter pool is somehow full, fix the code dummy");
        }

        letterSlot.SendLetterToSlot(_tmpSlot);
    }

    private void AnswerCheck()
    {

        string answer = "";
        //creates a string of all letters in answer slots
        foreach (var slot in AnswerSlots)
        {
            if (slot.IsEmpty)
            {
                return;// exits method if answer slots aren't full
            }
            answer += slot.CurrentLetter.LetterValue;
        }

        if (answer == CurrentQuestion.Answer)
        {
            print("Correct");
            if (_currentQuestionIndex < CurrentLevel.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                GenerateQuestion();
            }
            else
            {
                print("Level complete");
                //level complete
            }
        }
    }

    private void ResetPool()
    {
        foreach (var slot in AnswerSlots)
        {
            if (!slot.IsEmpty)
            {
                AnswerToPool(slot);
            }
        }
    }

    public void NextQueestion()
    {

    }

    void GenerateQuestion()
    {
        ResetPool();
        CurrentQuestion = CurrentLevel.Questions[_currentQuestionIndex];
        AdjustAnswerSlots();
        LetterPool.SetLetters();
    }

    private void AdjustAnswerSlots()
    {
        int slotsDelta = CurrentQuestion.Answer.Length - AnswerSlots.Count;

        if (slotsDelta >= 0)
        {
            for (int i = 0; i < slotsDelta; i++)
            {
                AnswerSlots.Add(Instantiate(_gameManager.prefabs.LetteSlotrPrefab, AnswerSlotsLayout));
            }
        }
        else
        {
            for (int i = 0; i > slotsDelta; i--)
            {
                Destroy(AnswerSlots[AnswerSlots.Count - 1].gameObject);
                AnswerSlots.RemoveAt(AnswerSlots.Count - 1);
            }
        }

        // Set the position of the RectTransform
        //AnswerSlotsLayout.position = new Vector2((canvas.position.x / 2) * -1, AnswerSlotsLayout.anchoredPosition.y);
    }

    private void CenterAnswerLayout()
    {

    }
}
