using System;
using BWolf.GameTasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTaskPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _anyActiveTextRenderer;

    [SerializeField]
    private TMP_Text _activeCountTextRenderer;

    [SerializeField]
    private Button _pauseButton;

    [SerializeField]
    private Button _continueButton;

    private void Start()
    {
        _pauseButton.onClick.AddListener(GameTask.PauseAll);
        _continueButton.onClick.AddListener(GameTask.ContinueAll);
        
        UpdateText();
    }

    private void FixedUpdate()
    {
        UpdateText();

        Func<GameObject, bool> predicate = go => go.layer == LayerMask.NameToLayer("UI");
        if (Input.GetKeyDown(KeyCode.Return) && GameTask.AnyActive(predicate))
            Debug.Log("There is a ui task in progress.");
    }

    private void UpdateText()
    {
        _anyActiveTextRenderer.text = $"Tasks Active: {GameTask.AnyActive()}";
        _activeCountTextRenderer.text = $"Task Count: {GameTask.Count(true)}";
    }
}
