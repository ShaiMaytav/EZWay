using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Points;
    public List<LevelData> Levels;
    public PrefabsSO Prefabs;
    public DataSO Data;
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public string link;
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
        Levels[0].isUnlocked = true;
    }

    public void UnlockNextLevel(LevelData currentLevel)
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            if (Levels[i] == currentLevel && i < Levels.Count - 1)
            {
                Levels[i + 1].isUnlocked = true;
            }
        }
    }

    public void CallLink()
    {
        Helper.OpenURLLink(link);
    }

    public void BuyHint()
    {
        Points -= Data.HintCost;
    }

    public void IncreasePoints()
    {
        Points += Data.QuestionReward;
    }
}
