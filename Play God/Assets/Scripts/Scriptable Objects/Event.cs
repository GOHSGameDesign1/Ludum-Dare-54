using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event")]
public class Event : ScriptableObject
{
    public float chanceOfHappening;

    public Sprite icon;

    [TextArea(15,20)]
    public string description;

    public float lowerDuration;
    public float upperDuration;

    public List<Effect> effects;
}
