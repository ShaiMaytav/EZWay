using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public List<LevelData> Levels;
    public static GameManager Instance { get{return _instance;}}
    private static GameManager _instance;

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
}
