using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAbilityBar : MonoBehaviour
{
    [SerializeField]
    private Abilities _abilities;

    private void Awake()
    {
        _abilities.ValueChanged += OnAbilityChanged;
    }

    private void OnAbilityChanged()
    {
        
    }
}
