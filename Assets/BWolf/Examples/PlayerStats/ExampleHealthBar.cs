using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleHealthBar : MonoBehaviour
{
    [SerializeField]
    private Points _health;

    private void Awake()
    {
        _health.ValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        
    }
}
