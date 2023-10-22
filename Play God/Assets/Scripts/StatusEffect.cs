using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Population,
    Plant,
    Mineral,
    Animal
}

[System.Serializable]
public class StatusEffect
{

    public Target target;
    public int value;
    public float changeInterval;

    public StatusEffect(Target target, int value, float changeInterval)
    {
        this.target = target;
        this.value = value;
        this.changeInterval = changeInterval;
    }

}
