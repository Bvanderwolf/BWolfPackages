// Created By: Benjamin van der Wolf
// Version: 1.1
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier used for increasing/decreasing a stat until a given condition has been met</summary>
    [Serializable]
    public class ConditionalStatModifier : StatModifier
    {
        [SerializeField, Min(0), Tooltip("The amount of value this modifier will modify each second while active")]
        private int valuePerSecond = 0;

        /// <summary>Default condition that will, when added to a system, run the modifier until infinity</summary>
        public readonly static Func<bool> DefaultCondition = () => false;

        private Func<bool> stopCondition;

        private float timePassed;

        private int currentValue;
        private int valueModified;

        /// <summary>Called each second the system has been modified, providing the name of this modifier as a string and the value modified as an integer</summary>
        public Action<string, int> OnSecondPassed;

        /// <summary>Returns whether the stopcondition has been met</summary>
        public override bool Finished
        {
            get { return stopCondition(); }
        }

        /// <summary>
        /// Creates a new instance of a conditinal stat modifier using all available options
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="valuePerSecond">The amount of value this modifier will modify each second while active</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="modifiesCurrentWithMax">Does current value change when max value has changed?</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        /// <param name="stopCondition">The condition on which this stat modifier needs to stop modifying</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax, bool canStack, Func<bool> stopCondition)
        {
            this.name = name;
            this.valuePerSecond = valuePerSecond;
            this.increase = increase;
            this.modifiesCurrent = modifiesCurrent;
            this.modifiesCurrentWithMax = modifiesCurrentWithMax;
            this.canStack = canStack;
            this.stopCondition = stopCondition ?? DefaultCondition;
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier using the default "ConditionalStatModifier.DefaultCondition" condition
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase) : this(name, valuePerSecond, increase, true, false, false, null)
        {
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier using a stopcondition
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase, Func<bool> stopCondition) : this(name, valuePerSecond, increase, true, false, false, stopCondition)
        {
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier using the default "ConditionalStatModifier.DefaultCondition" condition
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="modifiesCurrentWithMax">Does current value change when max value has changed?</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax = false) : this(name, valuePerSecond, increase, modifiesCurrent, modifiesCurrentWithMax, false, null)
        {
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier using a stopcondition
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="modifiesCurrentWithMax">Does current value change when max value has changed?</param>
        public ConditionalStatModifier(string name, int valuePerSecond, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax, Func<bool> stopCondition) : this(name, valuePerSecond, increase, modifiesCurrent, modifiesCurrentWithMax, false, stopCondition)
        {
        }

        /// <summary>Sets given condition as to when to stop this modifier</summary>
        public void SetStopCondition(Func<bool> stopCondition)
        {
            this.stopCondition = stopCondition;
        }

        /// <summary>Modifies system by regenerating or decaying given value, resseting and calling on second passed each second</summary>
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
                OnSecondPassed?.Invoke(name, valueModified);
                timePassed = 0;
                valueModified = 0;
            }
        }

        /// <summary>Returns the string representation of this conditional stat modifier</summary>
        public override string ToString()
        {
            return $"ConditionalStatModifier[name: {name}, valuePerSecond: {valuePerSecond}, increase: {increase}, modifiesCurrent: {modifiesCurrent}, canStack: {canStack}]";
        }
    }
}