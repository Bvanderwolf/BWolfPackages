using System.Collections;
using BWolf.GameTasks;
using UnityEngine;

public class MoveRightTask : MonoBehaviour
{
    [SerializeField]
    private float _movementRight = 100f;

    [SerializeField]
    private float _time = 5f;

    [SerializeField]
    private bool _ignorePause = false;

    private GameTask task;
    
    // Start is called before the first frame update
    void Start()
    {
        task = new GameTask(this, MoveToRight());
        task.Started += OnStart;
        task.Ended += OnEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !task.HasStarted)
            task.Start();
        
        if (Input.GetKeyDown(KeyCode.R) && task.HasEnded)
            task.Reset();
        
        if (Input.GetKeyDown(KeyCode.P) && task.IsActive && !_ignorePause)
            task.Pause();
        else if (Input.GetKeyDown(KeyCode.C) && task.IsPaused)
            task.Continue();
    }

    private void OnStart()
    {
        Debug.Log("Started moving right routine");
        Debug.Log($"Count: {GameTask.Count()}");
    }

    private void OnEnd()
    {
        Debug.Log("Ended moving right routine");
        Debug.Log($"Count: {GameTask.Count()}");
    }

    private IEnumerator MoveToRight()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition;
        float time = 0.0f;
        
        targetPosition.x += _movementRight;
        while (time < _time)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / _time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
