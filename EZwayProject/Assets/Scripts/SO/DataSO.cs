using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/Data", order = 2)]
public class DataSO : ScriptableObject
{
    public int HintCost;
    public int QuestionReward;
}
