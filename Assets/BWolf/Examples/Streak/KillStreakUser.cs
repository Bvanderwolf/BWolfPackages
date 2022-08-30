using System;
using BWolf.Utilities;
using UnityEngine;

public class KillStreakUser : MonoBehaviour
{
    [SerializeField]
    private Streak _killStreak;

    public void OnButtonClick()
    {
        StreakContinuation continuation = _killStreak.Continue();
        Debug.Log(continuation);
        if(continuation == StreakContinuation.REACHED_CEILING)
            _killStreak.Reset();
    }
}
