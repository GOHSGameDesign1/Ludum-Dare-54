using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect")]
public class Effect : ScriptableObject
{
    [Tooltip("The effect operation: 0 denotes addition, 1 denotes multiplication")]
    public int addOrTimes;

    public float value;

    [Tooltip("plant, mineral, animal, birth, death")]
    public string target;


    public bool isInstant;

    // 0 = increase rate, 1 = decrease rate, only for population
    public int incOrDec;
}
