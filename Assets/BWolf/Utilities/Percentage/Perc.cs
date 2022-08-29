using System;
using UnityEngine;

[Serializable]
public struct Perc
{
    public float value;
    
    public bool clamp01;

    public Perc(float value, bool clamp01 = true)
    {
        this.clamp01 = clamp01;
        this.value = clamp01 ? Mathf.Clamp01(value) : value;
    }

    public Perc Add(float value) => Set(this.value + value);

    public Perc Remove(float value) => Set(this.value - value);

    public Perc Set(float newValue) => new Perc(clamp01 ? Mathf.Clamp01(newValue) : newValue);
}
