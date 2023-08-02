using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsSO", menuName = "ScriptableObjects/Prefabs", order = 1)]
public class PrefabsSO : ScriptableObject
{
    public LetterObject LetterObjectPrefab;
    public LetterSlot LetteSlotrPrefab;
}
