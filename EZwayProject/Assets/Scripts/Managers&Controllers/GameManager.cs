using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int Points;
    public int TitleRank;
    public List<LevelData> Levels;
    public PrefabsSO Prefabs;
    public DataSO Data;
    public List<OfferData> Offers;
    public string WebsiteLink;
    [SerializeField] private LevelsParser levelsParser;
    [SerializeField] private SheetReader sheetReader;
    [SerializeField] private int FrameRate = 60;



    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public bool CanUseHint { get { return Points >= Data.HintCost; } }

    private void Awake()
    {
        #region Singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("A GameManager component was removed from " + gameObject.name);
            Destroy(this);
        }
        #endregion
    }

    private void Start()
    {
        Application.targetFrameRate = FrameRate;
        CheckConnection();
    }

    private void Init()
    {
        Levels = levelsParser.GetLevelsFromSheets();
        Offers = sheetReader.GetOffers();
        WebsiteLink = sheetReader.GetWebsiteLink();
        SyncProgression();
        Levels[0].IsUnlocked = true;
        Levels[0].DidOffer = true;
        //UIManager.Instance.UpdateLevelIcon();
    }

    [ContextMenu("Update title")]
    private void ManualUpdateTitle()
    {
        UIManager.Instance.UpdateLevelIcon();
    }

    public void UnlockNextLevel(LevelData currentLevel)
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            if (Levels[i] == currentLevel && i < Levels.Count - 1)
            {
                Levels[i + 1].IsUnlocked = true;
            }
        }
    }

    public void CallLink(string link)
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        Helper.OpenURLLink(link);
    }

    public void CallWebLink()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        Helper.OpenURLLink(WebsiteLink);
    }

    public void BuyHint()
    {
        Points -= Data.HintCost;
    }

    public void IncreasePoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    private void SyncProgression()
    {
        SaveLoad data = SaveLoad.Instance;
        data.Load();
        List<LevelProgression> levelsProgression = data.LevelsProgression;

        Points = data.SavedPoints;
        TitleRank = data.SavedTitleRank;

        //adjust number of levels from loaded data
        if (levelsProgression.Count < Levels.Count)
        {
            int size = Levels.Count - levelsProgression.Count;
            for (int i = 0; i < size; i++)
            {
                levelsProgression.Add(new LevelProgression());
            }
        }
        else if (levelsProgression.Count > Levels.Count)
        {
            int size = levelsProgression.Count - Levels.Count;
            for (int i = 0; i < size; i++)
            {
                levelsProgression.RemoveAt(levelsProgression.Count - 1);
            }
        }

        for (int i = 0; i < levelsProgression.Count; i++)
        {
            //decrease completed questions in loaded data if needed 
            if (levelsProgression[i].QuestionsCompleted > Levels[i].Questions.Count)
            {
                LevelProgression tmpLvlProg = levelsProgression[i];
                tmpLvlProg.QuestionsCompleted = Levels[i].Questions.Count;
                levelsProgression[i] = tmpLvlProg;
            }

            //sync level data
            Levels[i].CompletedQuestionsCount = levelsProgression[i].QuestionsCompleted;
            Levels[i].DidOffer = levelsProgression[i].didOffer;
        }

        //unlock levels
        for (int i = 0; i < Levels.Count - 1; i++)
        {
            if (Levels[i].IsCompleted)
            {
                Levels[i + 1].IsUnlocked = true;
            }
        }
        data.Save();
    }

    public void IncreaseTitleRank()
    {
        TitleRank++;
        UIManager.Instance.UpdateLevelIcon();
        SaveLoad.Instance.SavedTitleRank = TitleRank;
    }

    private void CheckConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Init();
            UIManager.Instance.Startup(true);
        }
        else
        {
            UIManager.Instance.Startup(false);
        }
    }

    public void RestartApp()
    {
        AudioManager._instance.PlaySFX(Sounds.UIClick);

        SceneManager.LoadScene(0);
    }
}
