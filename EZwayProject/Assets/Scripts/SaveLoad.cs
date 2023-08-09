using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public int num;
    public string name;

    [ContextMenu("poop")]
    public void Save()
    {
        string data = JsonUtility.ToJson(this);
        string path = Application.persistentDataPath + "SaveLoad.json";
        System.IO.File.WriteAllText(path, data);

    }

    [ContextMenu("pee")]
    public void Load()
    {
        string path = Application.persistentDataPath + "SaveLoad.json";
        string data = System.IO.File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(data, this);
    }
}
