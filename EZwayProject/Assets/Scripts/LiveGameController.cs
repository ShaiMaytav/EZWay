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
    public List<LetterSlot> AnswerSlots;
    public UnityEvent OnPoolLetterPicked;
    public bool canPlay;

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
            Debug.LogWarning("A LiveGameController component was removed from " + gameObject.name);
            Destroy(this);
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        OnPoolLetterPicked.AddListener(AnswerCheck);
    }

    private void OnEnable()
    {
        canPlay = true;
    }

    public void StartLevel(int levelNum)
    {
        CurrentLevel = _gameManager.Levels[levelNum];
        _currentQuestionIndex = CurrentLevel.CompletedQuestionsCount == CurrentLevel.Questions.Count ? 0 : CurrentLevel.CompletedQuestionsCount;
        GenerateQuestion();
    }

    public void PickLetter(LetterSlot letterSlot)
    {
        if (letterSlot.CurrentLetter != null && canPlay)
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
            GameManager.Instance.IncreasePoints();
            if (_currentQuestionIndex < CurrentLevel.Questions.Count - 1)
            {
                _currentQuestionIndex++;
                UIManager.Instance.QuestionComplete();
            }
            else
            {
                print("Level complete");
                UIManager.Instance.LevelComplete();
                GameManager.Instance.UnlockNextLevel(CurrentLevel);
            }
            CurrentLevel.CompletedQuestionsCount++;
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

    public void NextQueestion()//tbc//
    {
        GenerateQuestion();
    }

    public void NextLevel()
    {
        StartLevel(CurrentLevel.LevelNum); //didnt add 1 since levelnum is larger than its index by 1
        UIManager.Instance.NextLevel();
    }

    public void LevelComplete()//tbc//
    {
        UIManager.Instance.GameToLevelSelection();
    }

    void GenerateQuestion()
    {
        ResetPool();
        CurrentQuestion = CurrentLevel.Questions[_currentQuestionIndex];
        AdjustAnswerSlots();
        UIManager.Instance.UpdateQuestionText(CurrentQuestion.Example, CurrentQuestion.Condition, CurrentQuestion.Question);
        UIManager.Instance.UpdateQuestionsTrack((_currentQuestionIndex + 1) + "/" + CurrentLevel.Questions.Count);
        UIManager.Instance.UpdatePointsText();
        LetterPool.SetLetters();
    }

    private void AdjustAnswerSlots()
    {
        int slotsDelta = CurrentQuestion.Answer.Length - AnswerSlots.Count;

        if (slotsDelta >= 0)
        {
            for (int i = 0; i < slotsDelta; i++)
            {
                AnswerSlots.Add(Instantiate(_gameManager.Prefabs.LetteSlotrPrefab, AnswerSlotsLayout));
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

    public void Hint()
    {
        if (GameManager.Instance.CanUseHint && canPlay)
        {
            GameManager.Instance.BuyHint();
            LetterSlot _resSlot = null; //the slot containing the letter we need  
            LetterSlot _chosenSlot = null; //the answer slot chosen to be filled
            string _answerLetter = null; // need this to find the right resSlot
            List<int> _uncheckedAnswerSlotsIndexes = new List<int>();

            //populate list with indexes
            for (int i = 0; i < AnswerSlots.Count; i++)
            {
                _uncheckedAnswerSlotsIndexes.Add(i);
            }

            #region PickSlotToFill
            for (int i = AnswerSlots.Count; i > 0; i--)
            {
                int _index = Random.Range(0, i); // index for random indexes list (sorry)
                int _randomIndex = _uncheckedAnswerSlotsIndexes[_index];
                _chosenSlot = AnswerSlots[_randomIndex];
                _answerLetter = CurrentQuestion.Answer[_randomIndex].ToString();

                if (_chosenSlot.CurrentLetter == null)
                {
                    break;
                }

                if (_chosenSlot.CurrentLetter.LetterValue != _answerLetter)
                {
                    AnswerToPool(_chosenSlot);
                    break;
                }
                _uncheckedAnswerSlotsIndexes.RemoveAt(_index);//remove the index we used 
                _chosenSlot = null;
            }

            if (_chosenSlot == null)
            {
                Debug.LogError("Couldn't find an answer slot fill");
                return;
            }

            #endregion

            #region FindCorrectSlot
            foreach (var slot in LetterPool.AllSlots)
            {
                if (slot.CurrentLetter != null && slot.CurrentLetter.LetterValue == _answerLetter)
                {
                    _resSlot = slot;
                    break;
                }
            }

            if (_resSlot == null)
            {
                for (int i = 0; i < AnswerSlots.Count; i++)
                {
                    LetterSlot _currentSlot = AnswerSlots[i];
                    if (_currentSlot.CurrentLetter != null &&
                        _currentSlot.CurrentLetter.LetterValue != CurrentQuestion.Answer[i].ToString() && //answerslot letter is incorrect
                        _currentSlot.CurrentLetter.LetterValue == _answerLetter) //answerslot contains the letter we are looking for
                    {
                        _resSlot = _currentSlot;
                    }
                }
            }

            if (_resSlot == null)
            {
                Debug.LogError("Couldn't find an slot with wanted letter");
            }
            #endregion

            _resSlot.SendLetterToSlot(_chosenSlot);
            OnPoolLetterPicked.Invoke();
        }
    }
}
