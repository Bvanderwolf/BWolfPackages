using System;
using System.Collections;
using System.Collections.Generic;
using BWolf.GameTasks;
using UnityEngine;
using UnityEngine.UI;

public class FillBarTask : MonoBehaviour
{
    [SerializeField]
    private float _fillTime = 5f;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    private GameTask task;
    
    // Start is called before the first frame update
    void Start()
    {
        task = new GameTask(this, FillImage());
        task.Started += OnStart;
        task.Ended += OnEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !task.HasStarted)
            task.Start();
        
        if (Input.GetKeyDown(KeyCode.R) && task.HasEnded)
            task.Reset();
        
        if (Input.GetKeyDown(KeyCode.P) && task.IsActive)
            task.Pause();
        else if (Input.GetKeyDown(KeyCode.C) && task.IsPaused)
            task.Continue();
    }

    private void OnStart()
    {
        Debug.Log("Started fill bar routine");
        Debug.Log($"Count: {GameTask.Count()}");
    }

    private void OnEnd()
    {
        Debug.Log("Ended fill bar routine");
        Debug.Log($"Count: {GameTask.Count()}");
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
