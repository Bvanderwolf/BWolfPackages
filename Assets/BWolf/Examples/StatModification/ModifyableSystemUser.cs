﻿using BWolf.Utilities.StatModification;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class ModifyableSystemUser : MonoBehaviour
    {
        [Header("Stack System")]
        [SerializeField]
        protected StatSystem stackSystem = null;

        [SerializeField]
        protected ModifierInfoSO stackModifier = null;

        [SerializeField]
        protected Text txtStackSystemEventDisplay = null;

        [Header("Non Stack System")]
        [SerializeField]
        protected StatSystem nonStackSystem = null;

        [SerializeField]
        protected ModifierInfoSO nonStackModifier = null;

        [SerializeField]
        protected Text txtNonStackSystemEventDisplay = null;

        protected const float alphaCrossfadeTime = 3f;

        protected virtual void Awake()
        {
            stackSystem.SetCurrentToMax();

            stackSystem.OnDecreaseStart += () => ShowStackEventTextWithFade("Decrease Started");
            stackSystem.OnDecreaseStop += () => ShowStackEventTextWithFade("Decrease Stopped");
            stackSystem.OnReachedMax += () => ShowStackEventTextWithFade("Reached Max");
            stackSystem.OnReachedZero += () => ShowStackEventTextWithFade("Reached Zero");
            stackSystem.OnIncreaseStart += () => ShowStackEventTextWithFade("Increase Started");
            stackSystem.OnIncreaseStop += () => ShowStackEventTextWithFade("Increase Stopped");

            nonStackSystem.SetCurrentToMax();

            nonStackSystem.OnDecreaseStart += () => ShowNonStackEventTextWithFade("Decrease Started");
            nonStackSystem.OnDecreaseStop += () => ShowNonStackEventTextWithFade("Decrease Stopped");
            nonStackSystem.OnReachedMax += () => ShowNonStackEventTextWithFade("Reached Max");
            nonStackSystem.OnReachedZero += () => ShowNonStackEventTextWithFade("Reached Zero");
            nonStackSystem.OnIncreaseStart += () => ShowNonStackEventTextWithFade("Increase Started");
            nonStackSystem.OnIncreaseStop += () => ShowNonStackEventTextWithFade("Increase Stopped");
        }

        private void ShowStackEventTextWithFade(string text)
        {
            txtStackSystemEventDisplay.canvasRenderer.SetAlpha(1f);
            txtStackSystemEventDisplay.text = text;
            txtStackSystemEventDisplay.CrossFadeAlpha(0, alphaCrossfadeTime, false);
        }

        private void ShowNonStackEventTextWithFade(string text)
        {
            txtNonStackSystemEventDisplay.canvasRenderer.SetAlpha(1f);
            txtNonStackSystemEventDisplay.text = text;
            txtNonStackSystemEventDisplay.CrossFadeAlpha(0, alphaCrossfadeTime, false);
        }

        public virtual void OnValueChanged(string changedValue)
        {
        }

        public virtual void OnAddStackModifierButtonClick()
        {
        }

        public virtual void OnAddNonStackModifierButtonClick()
        {
        }

        public void OnStackModifierIncreaseToggled(bool value) => stackModifier.Increase = value;

        public void OnStackModifierModifyCurrentToggled(bool value) => stackModifier.ModifiesCurrent = value;

        public void OnStackModifierModifyCurrentWithMaxToggled(bool value) => stackModifier.ModifiesCurrentWithMax = value;

        public void OnNonStackModifierIncreaseToggled(bool value) => nonStackModifier.Increase = value;

        public void OnNonStackModifierModifyCurrentToggled(bool value) => nonStackModifier.ModifiesCurrent = value;

        public void OnNonStackModifierModifyCurrentWithMaxToggled(bool value) => nonStackModifier.ModifiesCurrentWithMax = value;
    }
}