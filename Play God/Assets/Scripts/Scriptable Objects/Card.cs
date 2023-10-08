using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card")]
public class Card : ScriptableObject
{
    public string title;
    public string description;

    public Sprite icon;

    public float lowerDurationLimit;
    public float upperDurationLimit;

    public List<Effect> effects;
}
