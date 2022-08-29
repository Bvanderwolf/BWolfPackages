using BWolf.Utilities.StatModification;
using UnityEngine;

namespace BWolf.Examples.StatModification
{
    public class TimedModifiyableSystemUser : ModifyableSystemUser
    {
        public override void OnValueChanged(string changedValue)
        {
            int value = int.Parse(changedValue);
            stackModifier.value = value;
            nonStackModifier.value = value;
        }

        public void OnTimeChanged(string changedTime)
        {
            float time = float.Parse(changedTime);
            if (time >= 0)
            {
                ((TimedModifierInfoSO)nonStackModifier).time = time;
                ((TimedModifierInfoSO)stackModifier).time = time;
            }
            else
            {
                Debug.LogWarning("time needs to be a positive number");
            }
        }

        public override void OnAddStackModifierButtonClick() => stackSystem.AddTimedModifier(stackModifier as TimedModifierInfoSO);

        public override void OnAddNonStackModifierButtonClick() => nonStackSystem.AddTimedModifier(nonStackModifier as TimedModifierInfoSO);
    }
}