using System;
using System.Collections;
using System.Collections.Generic;
using BWolf.PlayerStatistics;
using UnityEngine;

public class ExampleManaBar : MonoBehaviour
{
   [SerializeField]
   private PointsStat _mana;

   private void Awake()
   {
      _mana.ValueChanged += OnManaChanged;
   }

   private void OnManaChanged()
   {
      
   }
}
