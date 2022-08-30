# Streak

Provides users with a simple and extendable way to use continuable streak operation data.

## Features

- A serializable Streak class

## Usage examples
### Button click streak
```c#
using BWolf.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickListener
{
    private Button _button;
    
    private Streak _streak;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.OnClick.AddListener(OnButtonClick);
        
        _streak = new Streak();
        _streak.ceiling = 10;
        _streak.cooldown = 2f;
        _streak.interval = 1f;
    }
    
    private void OnButtonClick()
    {
        StreakContinuation continuation = _streak.Continue();
        switch (continuation)
        {
            case StreakContinuation.REACHED_CEILING:
                Debug.Log("I reached the ceiling of my streak");
                break;
            case StreakContinuation.ON_COOLDOWN:
                Debug.Log("I need to wait before I start my streak again");
                break;
            case StreakContinuation.MISSED_INTERVAL:
                Debug.Log("I was to late with my click. Now my streak is reset!");
                break;
            case StreakContinuation.SUCCESFULL:
                Debug.Log("I was on time with my click. My streak continues!");
                Debug.Log($"My streak is now: {_streak.Current}");
                break;
        }
    }
}
```