using System;
using UnityEngine.Events;

namespace BWolf.Utilities.StatModification
{
    [Serializable]
    public class SecondPassedEvent : UnityEvent<string, int>
    {
    }
}