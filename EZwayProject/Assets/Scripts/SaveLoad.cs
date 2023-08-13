using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{

    public List<LevelProgression> LevelsProgression = new List<LevelProgression>();

    private static SaveLoad _instance;
    public static SaveLoad Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("A SaveLoad component was removed from " + gameObject.name);
            Destroy(this);
        }
    }

    [ContextMenu("save")]
    public void Save()
    {
        Debug.Log("Saved Game");
        string data = JsonUtility.ToJson(this);
        string path = Application.persistentDataPath + "SaveLoad.json";
        System.IO.File.WriteAllText(path, data);

    }

    [ContextMenu("load")]
    public void Load()
    {
        Debug.Log("Tried Load Game");

        if (System.IO.File.Exists(Application.persistentDataPath + "SaveLoad.json"))
        {
            Debug.Log("Success Load Game");

            string path = Application.persistentDataPath + "SaveLoad.json";
            string data = System.IO.File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(data, this);
        }
    }
}

[System.Serializable]
public struct LevelProgression
{
    public int LevelNum;
    public int QuestionsCompleted;
}
