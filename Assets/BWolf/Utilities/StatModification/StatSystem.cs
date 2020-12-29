// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>the main class to be used to show specific stats e.g. hitpoints or energy and modify its state by modifying a current or a max value</summary>
    public class StatSystem : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private int max = 1000;

        private List<StatModifier> activeModifiers = new List<StatModifier>();
        private List<StatModifier> queuedModifiers = new List<StatModifier>();

        /// <summary>Fired once when current or max has started increasing</summary>
        public event Action OnIncreaseStart;

        /// <summary>Fired once when current or max has stopped increasing</summary>
        public event Action OnIncreaseStop;

        /// <summary>Fired once when current or max has started decreasing</summary>
        public event Action OnDecreaseStart;

        /// <summary>Fired once when current or max has stopped decreasing</summary>
        public event Action OnDecreaseStop;

        /// <summary>Fired once when current has reached the max value</summary>
        public event Action OnReachedMax;

        /// <summary>Fired once when current has reached zero</summary>
        public event Action OnReachedZero;

        /// <summary>Returns current value</summary>
        public int Current { get; private set; }

        /// <summary>Returns max value for this system</summary>
        public int Max
        {
            get { return max; }
        }

        /// <summary>Returns percentage of current and max</summary>
        public float Perc
        {
            get { return (float)Current / max; }
        }

        /// <summary>Returns whether current is equal to max value</summary>
        public bool IsFull
        {
            get { return Current == max; }
        }

        /// <summary>Returns whether this system is being modified by stat modifiers</summary>
        public bool BeingModified
        {
            get { return activeModifiers.Count != 0; }
        }

        private void Update()
        {
            if (!BeingModified)
            {
                return;
            }

            bool currentIsMax = Current == max;
            bool currentIsZero = Current == 0f;

            //let each modifier modify this system and remove it if it is finished giving callbacks if the condition is right
            for (int i = activeModifiers.Count - 1; i >= 0; i--)
            {
                StatModifier modifier = activeModifiers[i];
                modifier.Modify(this);
                if (modifier.Finished)
                {
                    RemoveActiveModifierInternal(modifier, i);
                }
            }

            CheckZeroMaxEvents(currentIsMax, currentIsZero);
        }

        /// <summary>Sets current to max without callbacks</summary>
        public void SetCurrentToMax()
        {
            Current = max;
        }

        /// <summary>Sets current to zero without callbacks</summary>
        public void SetCurrentToZero()
        {
            Current = 0;
        }

        /// <summary>Lets a modifier modify the current value</summary>
        public void ModifyCurrent(StatModifier modifier, int value)
        {
            if (activeModifiers.Contains(modifier))
            {
                if (value > 0 && Current != max)
                {
                    Current += value;
                    if (Current > max)
                    {
                        Current = max;
                    }
                }
                else if (value < 0 && Current != 0)
                {
                    Current += value;
                    if (Current < 0)
                    {
                        Current = 0;
                    }
                }
            }
        }

        /// <summary>Lets a modifier modify the max value</summary>
        public void ModifyMax(StatModifier modifier, int value)
        {
            if (activeModifiers.Contains(modifier))
            {
                if (value > 0)
                {
                    max += value;
                }
                else if (value < 0)
                {
                    max += value;
                    if (max < 0)
                    {
                        max = 0;
                    }
                    if (Current > max)
                    {
                        Current = max;
                    }
                }
            }
        }

        /// <summary>Removes all queued and active modifiers from system</summary>
        public void Clear()
        {
            //clear queue so no new active modifiers enter the system
            queuedModifiers.Clear();

            //remove all active modifiers in normal fashion to make sure stop events are called on increase and decrease
            for (int i = activeModifiers.Count - 1; i >= 0; i--)
            {
                RemoveActiveModifierInternal(activeModifiers[i], i);
            }
        }

        /// <summary>Removes first or all occurences of modifier with given name from the systems active modifiers list</summary>
        public void RemoveActiveModifier(string modifierName, bool allOccurences)
        {
            for (int i = activeModifiers.Count - 1; i >= 0; i--)
            {
                if (activeModifiers[i].name == modifierName)
                {
                    RemoveActiveModifierInternal(activeModifiers[i], i);
                    if (!allOccurences) { break; }
                }
            }
        }

        /// <summary>Removes first or all occurences of modifier with given name from the systems queued modifiers list</summary>
        public void RemoveQueuedModifier(string modifierName, bool allOccurences)
        {
            for (int i = queuedModifiers.Count - 1; i >= 0; i--)
            {
                if (queuedModifiers[i].name == modifierName)
                {
                    queuedModifiers.RemoveAt(i);
                    if (!allOccurences) { break; }
                }
            }
        }

        /// <summary>Updates queued modifiers based on given finished modifier at given index</summary>
        private void RemoveActiveModifierInternal(StatModifier modifier, int indexOfModifier)
        {
            if (queuedModifiers.Count != 0 && queuedModifiers.Any(m => m.name == modifier.name))
            {
                //if there are queued modifiers an one has the same name as this one, replace this one with the queued one
                for (int j = queuedModifiers.Count - 1; j >= 0; j--)
                {
                    if (queuedModifiers[j].name == modifier.name)
                    {
                        activeModifiers[indexOfModifier] = queuedModifiers[j];
                        queuedModifiers.RemoveAt(j);
                        break;
                    }
                }
            }
            else
            {
                //if this modifier cannot be replaced by a queued one, remove it
                activeModifiers.RemoveAt(indexOfModifier);
            }

            CheckStopEvents(modifier);
        }

        /// <summary>Checks whether stop events should be called</summary>
        private void CheckStopEvents(StatModifier modifier)
        {
            if (activeModifiers.Count(m => m.increase) == 0 && modifier.increase)
            {
                //if there are no more regenerating over time modifiers and this one (which was removed) was, the regeneration has ended
                OnIncreaseStop?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.increase) == 0 && !modifier.increase)
            {
                //if there are no more decrease over time modifiers and this one (which was removed) was, the decreasing has ended
                OnDecreaseStop?.Invoke();
            }
        }

        /// <summary>Checks whether zero or max reached events should be called</summary>
        private void CheckZeroMaxEvents(bool currentIsMax, bool currentIsZero)
        {
            //if current was not equal to max before modification but is now equal after modification, on reached max is called
            if (!currentIsMax && Current == max)
            {
                OnReachedMax?.Invoke();
            }

            //if current was not equal to zero before modification but is now equal after modification, on reached zero is called
            if (!currentIsZero && Current == 0f)
            {
                OnReachedZero?.Invoke();
            }
        }

        /// <summary>Checks if based on given modifier a start event should be fired</summary>
        private void CheckStartEvents(StatModifier modifier)
        {
            if (activeModifiers.Count(m => m.increase) == 0 && modifier.increase)
            {
                OnIncreaseStart?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.increase) == 0 && !modifier.increase)
            {
                OnDecreaseStart?.Invoke();
            }
        }

        /// <summary>Adds a timed modifier to the stat system based on given info. time defaults to 0</summary>
        public TimedStatModifier AddTimedModifier(TimedModifierInfoSO info)
        {
            if (info == null)
            {
                throw new InvalidOperationException("Timed modifier Info was null");
            }

            TimedStatModifier modifier = new TimedStatModifier(info);
            InsertModifierInSystem(modifier);
            return modifier;
        }

        /// <summary>Adds a conditional stat modifier to the system based on given info. stop conditoin defaults to null meaning it will never stop</summary>
        public ConditionalStatModifier AddConditionalModifier(ConditionalModifierInfoSO info)
        {
            if (info == null)
            {
                throw new InvalidOperationException("Conditional modifier Info was null");
            }

            ConditionalStatModifier modifier = new ConditionalStatModifier(info);
            InsertModifierInSystem(modifier);
            return modifier;
        }

        /// <summary>Inserts given modifier into the system either as active modifier or queued based on the can stack flag</summary>
        private void InsertModifierInSystem(StatModifier modifier)
        {
            //insert at index zero to make sure when removing a modifier by name, the first instance of this modifier inside the system is being removed
            if (!modifier.canStack && activeModifiers.Any(m => m.name == modifier.name))
            {
                queuedModifiers.Insert(0, modifier);
            }
            else
            {
                activeModifiers.Insert(0, modifier);
            }
        }
    }
}