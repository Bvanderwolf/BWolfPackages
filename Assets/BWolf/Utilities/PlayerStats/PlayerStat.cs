using System;
using UnityEngine;

public abstract class PlayerStat : ScriptableObject
{
    public event Action ValueChanged;

    public abstract void ResetToBaseValue();
    
    protected void OnValueChanged()
    {
        ValueChanged?.Invoke();
    }
}
