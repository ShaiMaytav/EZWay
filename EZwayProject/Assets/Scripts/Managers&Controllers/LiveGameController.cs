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

    private int _currentQuestionIndex;
    private GameManager _gameManager;
    private UIManager _uiManager;
    private AudioManager _audioManager;
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
        _uiManager = UIManager.Instance;
        _audioManager = AudioManager._instance;
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
            _uiManager.EnableOfferMenu();
            CurrentLevel.DidOffer = true;
            SaveLoad data = SaveLoad.Instance;
            LevelProgression tmpLvlProg = data.LevelsProgression[CurrentLevel.LevelNum - 1];
            tmpLvlProg.didOffer = CurrentLevel.DidOffer;
            data.LevelsProgression[CurrentLevel.LevelNum - 1] = tmpLvlProg;
            data.Save();
        }

        _uiManager.UpdateLevelIcon(CurrentLevel.LevelNum - 1);

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
                AnswerToPool(letterSlot, true);
            }

        }
    }

    private void PoolToAnswer(LetterSlot letterSlot)
    {
        _audioManager.PlaySFX(Sounds.InsertLetter);

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

        letterSlot.SendLetterToSlot(_tmpSlot, true);
        OnPoolLetterPicked.Invoke();
    }

    private void AnswerToPool(LetterSlot letterSlot, bool tween)
    {
        if (tween) { _audioManager.PlaySFX(Sounds.ExtractLetter); };

        LetterSlot _tmpSlot = LetterPool.EmptySlots[Random.Range(0, LetterPool.EmptySlots.Count)];

        if (_tmpSlot == null)
        {
            Debug.LogError("Letter pool is somehow full, fix the code dummy");
        }

        letterSlot.SendLetterToSlot(_tmpSlot, tween);
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
                _audioManager.PlaySFX(Sounds.QuestionComplete);

                _currentQuestionIndex++;
                //GenerateQuestion();
                UIManager.Instance.QuestionComplete(); //opens window between questions

            }
            else
            {
                print("Level complete");
                //first time completing a level
                if (!CurrentLevel.IsCompleted)
                {
                    _gameManager.IncreasePoints(_gameManager.Data.LevelReward);
                    //_gameManager.IncreaseTitleRank();
                }

                _uiManager.LevelComplete(CurrentLevel.LevelNum == GameManager.Instance.Levels.Count, !CurrentLevel.IsCompleted);
                _uiManager.UpdateLevelCompletionWindow(_gameManager.Data.LevelReward, CurrentLevel.LevelNum - 1);
                _gameManager.UnlockNextLevel(CurrentLevel);
            }

            if (!CurrentLevel.IsCompleted)
            {
                _gameManager.IncreasePoints(_gameManager.Data.QuestionReward);
                _uiManager.UpdatePointsText();

                CurrentLevel.CompletedQuestionsCount++;

                SaveLoad data = SaveLoad.Instance;
                LevelProgression tmpLvlProg = data.LevelsProgression[CurrentLevel.LevelNum - 1];
                tmpLvlProg.QuestionsCompleted++;
                data.LevelsProgression[CurrentLevel.LevelNum - 1] = tmpLvlProg;
                data.Save();
            }

            SaveLoad.Instance.SavedPoints = _gameManager.Points;
            SaveLoad.Instance.Save();
        }
        else
        {
            AudioManager._instance.PlaySFX(Sounds.Negative);
            _uiManager.WrongAnswer();
        }
    }

    private void ResetPool()
    {
        LeanTween.cancelAll();

        foreach (var slot in AnswerSlots)
        {
            if (!slot.IsEmpty)
            {
                AnswerToPool(slot, false);
            }
        }
    }

    public void NextQueestion()
    {
        GenerateQuestion();
        foreach (var slot in AnswerSlots)
        {
            slot.ChangeColors(_currentTheme);
        }

    }

    public void NextLevel()
    {
        _audioManager.PlaySFX(Sounds.UIClick);

        StartLevel(CurrentLevel.LevelNum); //didnt add 1 since levelnum is larger than its index by 1
        _uiManager.NextLevel();
    }

    public void EndLevel()
    {
        _audioManager.PlaySFX(Sounds.UIClick);

        _uiManager.GameToLevelSelection();
    }

    //displays current question and changes the letter pool 
    void GenerateQuestion()
    {
        ResetPool();
        CurrentQuestion = CurrentLevel.Questions[_currentQuestionIndex];
        AdjustAnswerSlots();
        _uiManager.UpdateHintSprite();
        _uiManager.UpdateQuestionText(CurrentQuestion.Example, CurrentQuestion.Condition, CurrentQuestion.Question);
        _uiManager.UpdateQuestionsTrack((_currentQuestionIndex + 1) + "/" + CurrentLevel.Questions.Count);
        _uiManager.UpdatePointsText();
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
        Vector3 newPos = AnswerSlotsLayout.position;
        newPos.x = AnswerSlotsLayout.transform.parent.position.x + offset;


        AnswerSlotsLayout.position = newPos;


        AnswerSlotsLayout.anchoredPosition = new Vector3(AnswerSlotsLayout.anchoredPosition.x / 100, AnswerSlotsLayout.anchoredPosition.y);


    }

    public void Hint()
    {
        if (GameManager.Instance.CanUseHint && canPlay)
        {
            _audioManager.PlaySFX(Sounds.UIClick);

            _gameManager.BuyHint();
            _uiManager.UpdatePointsText();
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
                    AnswerToPool(_chosenSlot, true);
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

            _resSlot.SendLetterToSlot(_chosenSlot, true);
            OnPoolLetterPicked.Invoke();
        }
        else
        {
            _uiManager.EnableNoPointsMenu();
        }
    }

    private void ChangeTheme()
    {
        //this makes sure that a level will habe a theme even if there its number is greater than the amount of themes
        int themeIndex = (CurrentLevel.LevelNum - 1) % _gameManager.Data.Themes.Count;
        
        _currentTheme = _gameManager.Data.Themes[themeIndex];

        _uiManager.ChangeUITheme(_currentTheme);

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
