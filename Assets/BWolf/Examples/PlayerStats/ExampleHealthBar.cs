using System;
using System.Collections;
using System.Collections.Generic;
using BWolf.PlayerStatistics;
using UnityEngine;

public class ExampleHealthBar : MonoBehaviour
{
    [SerializeField]
    private PointsStat _health;

    private void Awake()
    {
        _health.ValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        
    }
}
