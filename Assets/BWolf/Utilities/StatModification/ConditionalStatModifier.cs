// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier used for increasing/decreasing a stat until a given condition has been met</summary>
    [Serializable]
    public class ConditionalStatModifier : StatModifier
    {
        [SerializeField, Min(0), Tooltip("The amount of value this modifier will modify each second while active")]
        private int valuePerSecond = 0;

        /// <summary>Default condition that will, run the modifier until infinity</summary>
        public readonly static ModificationEndCondition DefaultCondition = () => false;

        private float timePassed;

        private int currentValue;
        private int valueModified;

        private SecondPassedEvent OnSecondPassedEvent;
        private ModificationEndCondition StopCondition;

        /// <summary>Returns whether the stopcondition has been met</summary>
        public override bool Finished
        {
            get { return StopCondition(); }
        }

        /// <summary>
        /// Creates a new instance of a conditinal stat modifier using a conditional modifier info scriptable object
        /// </summary>
        public ConditionalStatModifier(ConditionalModifierInfoSO info)
        {
            name = info.name;
            valuePerSecond = info.Value;
            increase = info.Increase;
            modifiesCurrent = info.ModifiesCurrent;
            modifiesCurrentWithMax = info.ModifiesCurrentWithMax;
            canStack = info.CanStack;

            StopCondition = info.StopCondition ?? DefaultCondition;
            OnSecondPassedEvent = info.OnSecondsPassed;
        }

        /// <summary>Sets given condition as to when to stop this modifier</summary>
        public ConditionalStatModifier ModifyUntil(ModificationEndCondition stopCondition)
        {
            StopCondition = stopCondition;
            return this;
        }

        /// <summary>Executes function each second the system has been modified, providing the name of this modifier as a string and the value modified as an integer</summary>
        public ConditionalStatModifier OnSecondPassed(UnityAction<string, int> callback)
        {
            OnSecondPassedEvent.AddListener(callback);
            return this;
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
                OnSecondPassedEvent?.Invoke(name, valueModified);
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