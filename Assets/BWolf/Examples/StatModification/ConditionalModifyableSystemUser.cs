using BWolf.Utilities.StatModification;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class ConditionalModifyableSystemUser : ModifyableSystemUser
    {
        [Header("References")]
        [SerializeField]
        private Dropdown stopConditionDropdown = null;

        [SerializeField]
        private Dropdown onSecondsPassedDropdown = null;

        [SerializeField]
        private InputField valueInput = null;

        private ModificationEndCondition stopCondition;
        private UnityAction<string, int> onSecondPassed;

        [Header("Stored Values")]
        [SerializeField]
        private string storedValue;

        [SerializeField]
        private string storedStopCondition;

        [SerializeField]
        private string storedOnSecondsPassedCondition;

        private string startValue;
        private string startStopCondition;
        private string startOnSecondsPassed;

        protected override void Awake()
        {
            base.Awake();

            storedValue = valueInput.text;
            storedStopCondition = stopConditionDropdown.captionText.text;
            storedOnSecondsPassedCondition = onSecondsPassedDropdown.captionText.text;

            OnStopConditionChange(0);
            OnSecondPassedChanged(0);
        }

        public override void OnValueChanged(string changedValue)
        {
            if (!string.IsNullOrEmpty(changedValue))
            {
                int value = int.Parse(changedValue);
                stackModifier.Value = value;
                nonStackModifier.Value = value;
                storedValue = changedValue;
            }
        }

        public void OnStopConditionChange(int value)
        {
            switch (stopConditionDropdown.captionText.text)
            {
                case "ValueEdited": stopCondition = () => valueInput.text != startValue; break;
                case "StopConditionChanged": stopCondition = () => stopConditionDropdown.captionText.text != startStopCondition; break;
                case "SecondPassedChanged": stopCondition = () => onSecondsPassedDropdown.captionText.text != startOnSecondsPassed; break;
            }
        }

        public void OnSecondPassedChanged(int value)
        {
            switch (onSecondsPassedDropdown.captionText.text)
            {
                case "ConsoleLog": onSecondPassed = (name, modifyValue) => Debug.Log($"Modifier {name} modified {modifyValue} value"); break;
                case "ConsoleError": onSecondPassed = (name, modifyValue) => Debug.LogError($"Modifier {name} modified {modifyValue} value"); break;
                case "ConsoleWarning": onSecondPassed = (name, modifyValue) => Debug.LogWarning($"Modifier {name} modified {modifyValue} value"); break;
            }
        }

        public override void OnAddStackModifierButtonClick()
        {
            startValue = valueInput.text;
            startStopCondition = stopConditionDropdown.captionText.text;
            startOnSecondsPassed = onSecondsPassedDropdown.captionText.text;

            stackSystem.AddConditionalModifier(stackModifier as ConditionalModifierInfoSO)
                .ModifyUntil(stopCondition)
                .OnSecondPassed(onSecondPassed);
        }

        public override void OnAddNonStackModifierButtonClick()
        {
            startValue = valueInput.text;
            startStopCondition = stopConditionDropdown.captionText.text;
            startOnSecondsPassed = onSecondsPassedDropdown.captionText.text;

            nonStackSystem.AddConditionalModifier(nonStackModifier as ConditionalModifierInfoSO)
                .ModifyUntil(stopCondition)
                .OnSecondPassed(onSecondPassed);
        }
    }
}