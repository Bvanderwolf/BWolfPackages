using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleManaBar : MonoBehaviour
{
   [SerializeField]
   private Points _mana;

   private void Awake()
   {
      _mana.ValueChanged += OnManaChanged;
   }

   private void OnManaChanged()
   {
      
   }
}
