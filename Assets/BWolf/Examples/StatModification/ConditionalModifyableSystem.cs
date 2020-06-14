using System;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class ConditionalModifyableSystem : ModifyableSystem
    {
        [Header("References")]
        [SerializeField]
        private Dropdown stopConditionDropdown = null;

        [SerializeField]
        private Dropdown onSecondsPassedDropdown = null;

        private Func<bool> stopCondition;
        private Action<string, int> onSecondPassed;

        public override void OnValueChanged(string changedValue)
        {
        }

        public override void OnTimeChanged(string changedTime)
        {
        }

        public override void OnAddStackModifierButtonClick() => stackSystem.AddConditionalModifier(stackModifier, stopCondition).OnSecondPassed += onSecondPassed;

        public override void OnAddNonStackModifierButtonClick() => nonStackSystem.AddConditionalModifier(nonStackModifier, stopCondition).OnSecondPassed += onSecondPassed;
    }
}