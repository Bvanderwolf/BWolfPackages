# GameTasks

An extension on unity coroutines. Game tasks should be easier to manage. The static 
api also provides additional info on the global state of game tasks in the app.

## Features
- GameTask instance part with methods
  - Start
  - Pause
  - Continue
- GameTask static part with Management and Query methods
  - Run
  - PauseAll
    - All
    - On MonoBehaviour
    - On GameObject
    - Using a predicate function
  - ContinueAll
    - All
    - On MonoBehaviour
    - On GameObject
    - Using a predicate function
  - Count
    - All
    - On MonBehaviour
    - On GameObject
    - Using a predicate function
  - AnyActive
    - All
    - On MonoBehaviour
    - On GameObject
    - Using a predicate function

## Usage Examples
### Moving an object to the right
```c#
using BWolf.GameTasks;
using UnityEngine;

public class MoveRightTask : MonoBehaviour
{
    [SerializeField]
    private float _movementRight = 100f;

    [SerializeField]
    private float _totalTime = 5f;
    
    private GameTask task;
    
    private void Start()
    {
        task = new GameTask(this, MoveToRight());
        task.Start();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && task.IsActive)
            task.Pause();
        else if (Input.GetKeyDown(KeyCode.C) && task.IsPaused)
            task.Continue();
    }
    
    private IEnumerator MoveToRight()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition;
        float time = 0.0f;
        
        targetPosition.x += _movementRight;
        while (time < _totalTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / _totalTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
```
### Filling up an image using fill amount and the static API
```c#
using BWolf.GameTasks;
using UnityEngine;
using UnityEngine.UI;

public class FillImageTask : MonoBehaviour
{
    [SerializeField]
    private float _fillTime = 5f;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    private void Start()
    {
        GameTask.Run(this, FillImage());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && GameTask.AnyActive(this))
            GameTask.PauseAll(this);
        else if (Input.GetKeyDown(KeyCode.C) && !GameTask.AnyActive(this))
            GameTask.ContinueAll(this);
            
    }
    
    private IEnumerator FillImage()
    {
        float time = 0.0f;
        while (time < _fillTime)
        {
            _image.fillAmount = Mathf.Lerp(0.0f, 1.0f, time / _fillTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
```
### Checking for ui tasks using the AnyActive method and a predicate
```c#
using BWolf.GameTasks;
using UnityEngine;
using UnityEngine.UI;

public class UITaskChecker : MonoBehaviour
{
    private void Update()
    {
        // A log is being done if the user presses the return key while a game object on
        // the UI layer has a game task running.
        Func<GameObject, bool> predicate = go => go.layer == LayerMask.NameToLayer("UI");
        if (Input.GetKeyDown(KeyCode.Return) && GameTask.AnyActive(predicate))
            Debug.Log("There is a ui task in progress.");
    }
}
```

