using System;
using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier used for increasing/decreasing stat until a given condition has been met</summary>
    [Serializable]
    public class ConditionalStatModifier : StatModifier
    {
        [SerializeField, Min(0), Tooltip("The amount of value this modifier will modify each second while active")]
        private int valuePerSecond = 0;

        private Func<bool> stopCondition;
        private Action<string, int> onSecondPassed;

        private float timePassed;

        private int currentValue;
        private int valueModified;

        /// <summary>Returns whether the condition has been met</summary>
        public override bool Finished
        {
            get { return stopCondition(); }
        }

        /// <summary>Returns whether value per second is valid</summary>
        public bool HasValidValuePerSecond
        {
            get { return valuePerSecond >= 0; }
        }

        /// <summary>Returns whether condition is valid (doesn't throw exception)</summary>
        public bool HasValidCondition
        {
            get
            {
                try
                {
                    stopCondition();
                }
                catch (Exception e)
                {
                    Debug.LogWarning("condition threw exception: " + e);
                    return false;
                }
                return true;
            }
        }

        /// <summary>Returns a new instance of this modifier. Use this if you have stored a modifier and want to re-use it</summary>
        public override StatModifier Clone
        {
            get
            {
                return new ConditionalStatModifier(name, valuePerSecond, increase, modifiesCurrent, modifiesCurrentWithMax, canStack, stopCondition, onSecondPassed);
            }
        }

        /// <summary>
        /// Creates a new instance of a conditinal stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="valuePerSecond">The amount of value this modifier will modify each second while active</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        /// <param name="stopCondition">The condition on which this stat modifier needs to stop modifying</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax, bool canStack, Func<bool> stopCondition, Action<string, int> onSecondPassed = null)
        {
            this.name = name;
            this.valuePerSecond = valuePerSecond;
            this.increase = increase;
            this.modifiesCurrent = modifiesCurrent;
            this.modifiesCurrentWithMax = modifiesCurrentWithMax;
            this.canStack = canStack;
            this.stopCondition = stopCondition;
            this.onSecondPassed = onSecondPassed;
        }

        /// <summary>Sets given condition as to when to stop this modifier</summary>
        public void SetStopCondition(Func<bool> stopCondition)
        {
            this.stopCondition = stopCondition;
        }

        /// <summary>Subscribes action to on second passed event</summary>
        public void AddOnSecondPassedListener(Action<string, int> action)
        {
            onSecondPassed += action;
        }

        /// <summary>Modifies system by regenerating or decaying given value each second</summary>
        public override void Modify(StatSystem system)
        {
            timePassed += Time.deltaTime;
            currentValue = (int)(timePassed * valuePerSecond);
            if (currentValue != valueModified)
            {
                int difference = Mathf.Abs(currentValue - valueModified);
                valueModified += difference;
                if (modifiesCurrent)
                {
                    system.ModifyCurrent(this, increase ? difference : -difference);
                }
                else
                {
                    system.ModifyMax(this, increase ? difference : -difference);
                    if (modifiesCurrentWithMax && !system.IsFull)
                    {
                        system.ModifyCurrent(this, increase ? difference : -difference);
                    }
                }
            }
            if (timePassed >= 1f)
            {
                onSecondPassed?.Invoke(name, valueModified);
                timePassed = 0;
                valueModified = 0;
            }
        }

        public override string ToString()
        {
            return $"ConditionalStatModifier[name: {name}, valuePerSecond: {valuePerSecond}, increase: {increase}, modifiesCurrent: {modifiesCurrent}, canStack: {canStack}]";
        }
    }
}