using UnityEngine;

namespace BWolf.Examples.StatModification
{
    public class TimedModifiyableSystem : ModifyableSystem
    {
        private float time = 0;

        public override void OnValueChanged(string changedValue)
        {
            int value = int.Parse(changedValue);
            stackModifier.Value = value;
            nonStackModifier.Value = value;
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

        public override void OnAddStackModifierButtonClick() => stackSystem.AddTimedModifier(stackModifier, time);

        public override void OnAddNonStackModifierButtonClick() => nonStackSystem.AddTimedModifier(nonStackModifier, time);
    }
}