using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiveGameController : MonoBehaviour
{
    public bool canPlay;
    public LetterPool LetterPool;
    public QuestionData CurrentQuestion;
    public LevelData CurrentLevel;
    public RectTransform AnswerSlotsLayout;
    public List<LetterSlot> AnswerSlots;
    public UnityEvent OnPoolLetterPicked;

    [SerializeField] private RectTransform canvas;
    [SerializeField] private float answerCenterOffset = 150;
    [SerializeField] private float answerSlotGap = 185;
    [SerializeField] private float asnwerSlotSpacingLayout = 85;

    private int _currentQuestionIndex;
    private GameManager _gameManager;
    private UITheme _currentTheme;


    public GameObject inermediateParent;

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

        //start from last unanswered question, if level completed start from begining
        _currentQuestionIndex = CurrentLevel.CompletedQuestionsCount == CurrentLevel.Questions.Count ? 0 : CurrentLevel.CompletedQuestionsCount;

        if (!CurrentLevel.DidOffer)
        {
            UIManager.Instance.EnableOfferMenu();
            CurrentLevel.DidOffer = true;
            SaveLoad data = SaveLoad.Instance;
            LevelProgression tmpLvlProg = data.LevelsProgression[CurrentLevel.LevelNum - 1];
            tmpLvlProg.didOffer = CurrentLevel.DidOffer;
            data.LevelsProgression[CurrentLevel.LevelNum - 1] = tmpLvlProg;
            data.Save();
        }

        GenerateQuestion();
        ChangeTheme();
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
        AudioManager._instance.PlaySFX(Sounds.InsertLetter);

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
        AudioManager._instance.PlaySFX(Sounds.ExtractLetter);

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
                AudioManager._instance.PlaySFX(Sounds.QuestionComplete);

                _currentQuestionIndex++;
                GenerateQuestion();

                //Update answers slots color ////////////TBI////////////////
                foreach (var slot in AnswerSlots)
                {
                    slot.ChangeColors(_currentTheme);
                }


                //UIManager.Instance.QuestionComplete(); //opens window between questions


            }
            else
            {
                print("Level complete");
                //first time completing a level
                if (!CurrentLevel.IsCompleted)
                {
                    _gameManager.IncreasePoints(_gameManager.Data.LevelReward);
                    _gameManager.IncreaseTitleRank();
                }

                UIManager.Instance.LevelComplete(CurrentLevel.LevelNum == GameManager.Instance.Levels.Count, !CurrentLevel.IsCompleted);
                UIManager.Instance.UpdateLevelCompletionWindow(_gameManager.Data.LevelReward);
                GameManager.Instance.UnlockNextLevel(CurrentLevel);
            }

            if (!CurrentLevel.IsCompleted)
            {
                GameManager.Instance.IncreasePoints(_gameManager.Data.QuestionReward);
                UIManager.Instance.UpdatePointsText();

                CurrentLevel.CompletedQuestionsCount++;

                SaveLoad data = SaveLoad.Instance;
                LevelProgression tmpLvlProg = data.LevelsProgression[CurrentLevel.LevelNum - 1];
                tmpLvlProg.QuestionsCompleted++;
                data.LevelsProgression[CurrentLevel.LevelNum - 1] = tmpLvlProg;
                data.Save();
            }

            SaveLoad.Instance.SavedPoints = _gameManager.Points;
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
        GenerateQuestion();

       
    }

    public void NextLevel()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        StartLevel(CurrentLevel.LevelNum); //didnt add 1 since levelnum is larger than its index by 1
        UIManager.Instance.NextLevel();
    }

    public void EndLevel()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        UIManager.Instance.GameToLevelSelection();
    }

    //displays current question and changes the letter pool 
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
                LetterSlot newSlot = Instantiate(_gameManager.Prefabs.LetteSlotrPrefab, AnswerSlotsLayout);
                AnswerSlots.Add(newSlot);
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

        CenterAnswerLayout();
    }

    [ContextMenu("Test now")]
    private void CenterAnswerLayout()
    {
        float offset = answerCenterOffset * (CurrentQuestion.Answer.Length - 1);
        Vector3 newPos = AnswerSlotsLayout.anchoredPosition;
        newPos.x = Screen.width / 2 + offset;

        float X = answerSlotGap * CurrentQuestion.Answer.Length;
        float X2 = X / 2;
        float Y = asnwerSlotSpacingLayout / 2;

        newPos.x -= X2;
        newPos.x -= Y;


        AnswerSlotsLayout.anchoredPosition = newPos;








    }

    public void Hint()
    {
        if (GameManager.Instance.CanUseHint && canPlay)
        {
            AudioManager._instance.PlaySFX(Sounds.UIClick);

            GameManager.Instance.BuyHint();
            UIManager.Instance.UpdatePointsText();
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
        else
        {
            UIManager.Instance.EnableNoPointsMenu();
        }
    }

    private void ChangeTheme()
    {
        //this makes sure that a level will habe a theme even if there its number is greater than the amount of themes
        int themeIndex = (CurrentLevel.LevelNum - 1) % _gameManager.Data.Themes.Count;
        
        _currentTheme = _gameManager.Data.Themes[themeIndex];

        UIManager.Instance.ChangeUITheme(_currentTheme);

        //change letterpool slots and letter colors
        foreach (var slot in LetterPool.AllSlots)
        {
            slot.ChangeColors(_currentTheme);
            slot.CurrentLetter.ChangeColors(_currentTheme);
        }

        //change answers slots color
        foreach (var slot in AnswerSlots)
        {
            slot.ChangeColors(_currentTheme);
        }

    }
}
