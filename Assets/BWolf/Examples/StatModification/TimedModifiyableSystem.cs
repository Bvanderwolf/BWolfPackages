using BWolf.Utilities.StatModification;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.StatModification
{
    public class TimedModifiyableSystem : MonoBehaviour
    {
        [Header("Stack System")]
        [SerializeField]
        private StatSystem stackSystem = null;

        [SerializeField]
        private StatModifierInfo timedStackModifier = new StatModifierInfo();

        [SerializeField]
        private Image imgStackSystemFillable = null;

        [SerializeField]
        private Text txtStackSystemDisplayText = null;

        [SerializeField]
        private Text txtStackSystemEventDisplay = null;

        [Header("Non Stack System")]
        [SerializeField]
        private StatSystem nonStackSystem = null;

        [SerializeField]
        private StatModifierInfo timedNonStackModifier = new StatModifierInfo();

        [SerializeField]
        private Image imgNonStackSystemFillable = null;

        [SerializeField]
        private Text txtNonStackSystemDisplayText = null;

        [SerializeField]
        private Text txtNonStackSystemEventDisplay = null;

        private const float alphaCrossfadeTime = 3f;

        private float time = 0;

        private void Awake()
        {
            stackSystem.SetCurrentToMax();
            stackSystem.AttachFillableBar(imgStackSystemFillable);
            stackSystem.AttachDisplayText(txtStackSystemDisplayText);

            stackSystem.OnDecreaseStart += () => ShowStackEventTextWithFade("Decrease Started");
            stackSystem.OnDecreaseStop += () => ShowStackEventTextWithFade("Decrease Stopped");
            stackSystem.OnReachedMax += () => ShowStackEventTextWithFade("Reached Max");
            stackSystem.OnReachedZero += () => ShowStackEventTextWithFade("Reached Zero");
            stackSystem.OnIncreaseStart += () => ShowStackEventTextWithFade("Increase Started");
            stackSystem.OnIncreaseStop += () => ShowStackEventTextWithFade("Increase Stopped");

            nonStackSystem.SetCurrentToMax();
            nonStackSystem.AttachFillableBar(imgNonStackSystemFillable);
            nonStackSystem.AttachDisplayText(txtNonStackSystemDisplayText);

            nonStackSystem.OnDecreaseStart += () => txtNonStackSystemEventDisplay.text = "Decrease Started";
            nonStackSystem.OnDecreaseStop += () => txtNonStackSystemEventDisplay.text = "Decrease Stopped";
            nonStackSystem.OnReachedMax += () => txtNonStackSystemEventDisplay.text = "Reached Max";
            nonStackSystem.OnReachedZero += () => txtNonStackSystemEventDisplay.text = "Reached Zero";
            nonStackSystem.OnIncreaseStart += () => txtNonStackSystemEventDisplay.text = "Increase Started";
            nonStackSystem.OnIncreaseStop += () => txtNonStackSystemEventDisplay.text = "Increase Stopped";
        }

        private void Update()
        {
            stackSystem.UpdateModifiers();
            stackSystem.UpdateVisuals();

            nonStackSystem.UpdateModifiers();
            nonStackSystem.UpdateVisuals();
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

        public void OnValueChanged(string changedValue)
        {
            int value = int.Parse(changedValue);
            timedStackModifier.Value = value;
            timedNonStackModifier.Value = value;
        }

        public void OnTimeChanged(string changedTime)
        {
            float time = float.Parse(changedTime);
            if (time >= 0)
            {
                this.time = time;
            }
            else
            {
                Debug.LogWarning("time needs to be a positive number");
            }
        }

        public void OnStackModifierIncreaseToggled(bool value) => timedStackModifier.Increase = value;

        public void OnStackModifierModifyCurrentToggled(bool value) => timedStackModifier.ModifiesCurrent = value;

        public void OnStackModifierModifyCurrentWithMaxToggled(bool value) => timedStackModifier.ModifiesCurrentWithMax = value;

        public void OnAddStackModifierButtonClick() => stackSystem.AddTimedModifier(timedStackModifier, time);

        public void OnNonStackModifierIncreaseToggled(bool value) => timedNonStackModifier.Increase = value;

        public void OnNonStackModifierModifyCurrentToggled(bool value) => timedNonStackModifier.ModifiesCurrent = value;

        public void OnNonStackModifierModifyCurrentWithMaxToggled(bool value) => timedNonStackModifier.ModifiesCurrentWithMax = value;

        public void OnAddNonStackModifierButtonClick() => nonStackSystem.AddTimedModifier(timedNonStackModifier, time);
    }
}