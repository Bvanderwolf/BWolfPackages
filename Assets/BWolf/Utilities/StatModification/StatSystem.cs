﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.StatModification
{
    /// <summary>Can be used to show specific stats e.g. hitpoints or energy and modify its state</summary>
    [Serializable]
    public class StatSystem
    {
        [SerializeField, Min(0)]
        private int max = 1000;

        private List<StatModifier> activeModifiers = new List<StatModifier>();
        private List<StatModifier> queuedModifiers = new List<StatModifier>();

        private Image fillableBar;

        private Text displayText;

        private float current;

        public event Action OnRegenStart, OnRegenStop;

        public event Action OnIncreaseStart, OnDecreaseStart;

        public event Action OnReachedMax, OnReachedZero;

        /// <summary>Returns current state value rounded to nearest integer value</summary>
        public int Current
        {
            get { return Mathf.RoundToInt(current); }
        }

        /// <summary>Returns max value for this system</summary>
        public int Max
        {
            get { return max; }
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

        /// <summary>Updates systems state based on modifiers stored</summary>
        public void UpdateModifiers()
        {
            bool currentIsMax = current == max;
            bool currentIsZero = current == 0f;

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

        /// <summary>shows feedback of data on fillablebar and text. Make sure these are not null before using this</summary>
        public void UpdateVisuals()
        {
            fillableBar.fillAmount = current / max;
            displayText.text = $"{Current}/{max}";
        }

        /// <summary>Sets current to max without callbacks</summary>
        public void SetCurrentToMax()
        {
            current = max;
        }

        /// <summary>Sets current to zero without callbacks</summary>
        public void SetCurrentToZero()
        {
            current = 0;
        }

        /// <summary>Lets a modifier that is in the list of modifiers modify current</summary>
        public void ModifyCurrent(StatModifier modifier, int value)
        {
            if (activeModifiers.Contains(modifier))
            {
                if (value > 0 && current != max)
                {
                    current += value;
                    if (current > max)
                    {
                        current = max;
                    }
                }
                else if (value < 0 && current != 0)
                {
                    current += value;
                    if (current < 0)
                    {
                        current = 0;
                    }
                }
            }
        }

        /// <summary>Lets a modifier that is in the list of modifiers modify max</summary>
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
                    if (current > max)
                    {
                        current = max;
                    }
                }
            }
        }

        /// <summary>Sets fillable bar reference if given image is of type filled</summary>
        public void AttachFillableBar(Image fillableImage)
        {
            if (fillableImage != null && fillableImage.type == Image.Type.Filled)
            {
                this.fillableBar = fillableImage;
            }
        }

        /// <summary>Sets display text reference</summary>
        public void AttachDisplayText(Text displayText)
        {
            if (displayText != null)
            {
                this.displayText = displayText;
            }
        }

        /// <summary>Resets members for displaying system state</summary>
        public void ResetReferences()
        {
            fillableBar = null;
            displayText = null;
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
                if (activeModifiers[i].Name == modifierName)
                {
                    RemoveActiveModifierInternal(activeModifiers[i], i);
                    if (!allOccurences) { break; }
                }
            }
        }

        /// <summary>Updates queued modifiers based on given finished modifier at given index</summary>
        private void RemoveActiveModifierInternal(StatModifier modifier, int indexOfModifier)
        {
            if (queuedModifiers.Count != 0 && queuedModifiers.Any(m => m.Name == modifier.Name))
            {
                //if there are queued modifiers an one has the same name as this one, replace this one with the queued one
                for (int j = queuedModifiers.Count - 1; j >= 0; j--)
                {
                    if (queuedModifiers[j].Name == modifier.Name)
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
            if (activeModifiers.Count(m => m.Increase) == 0 && modifier.Increase)
            {
                //if there are no more regenerating over time modifiers and this one (which was removed) was, the regeneration has ended
                OnRegenStop?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.Increase) == 0 && !modifier.Increase)
            {
                //if there are no more decrease over time modifiers and this one (which was removed) was, the decreasing has ended
                OnDecreaseStart?.Invoke();
            }
        }

        /// <summary>Checks whether zero or max reached events should be called</summary>
        private void CheckZeroMaxEvents(bool currentIsMax, bool currentIsZero)
        {
            //if current was not equal to max before modification but is now equal after modification, on reached max is called
            if (!currentIsMax && current == max)
            {
                OnReachedMax?.Invoke();
            }

            //if current was not equal to zero before modification but is now equal after modification, on reached zero is called
            if (!currentIsZero && current == 0f)
            {
                OnReachedZero?.Invoke();
            }
        }

        /// <summary>Checks if based on given modifier a start event should be fired</summary>
        private void CheckStartEvents(StatModifier modifier)
        {
            if (activeModifiers.Count(m => m.Increase) == 0 && modifier.Increase)
            {
                OnRegenStart?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.Increase) == 0 && !modifier.Increase)
            {
                OnIncreaseStart?.Invoke();
            }
        }

        /// <summary>Adds a timed modifier to the stat system based on given info. time defaults to 0</summary>
        public TimedStatModifier AddTimedModifier(StatModifierInfo info, float time = 0)
        {
            if (string.IsNullOrEmpty(info.Name))
            {
                return null;
            }
            else
            {
                TimedStatModifier modifier = new TimedStatModifier(info.Name, time, info.Value, info.Increase, info.ModifiesCurrent, info.ModifiesCurrentWithMax, info.CanStack);
                InsertModifierInSystem(modifier);
                return modifier;
            }
        }

        /// <summary>Adds a conditional stat modifier to the system based on given info. stop conditoin defaults to null meaning it will never stop</summary>
        public ConditionalStatModifier AddConditionalModifier(StatModifierInfo info, Func<bool> stopCondition = null)
        {
            if (string.IsNullOrEmpty(info.Name))
            {
                return null;
            }
            else
            {
                ConditionalStatModifier modifier = new ConditionalStatModifier(info.Name, info.Value, info.Increase, info.ModifiesCurrent, info.ModifiesCurrentWithMax, info.CanStack, stopCondition);
                InsertModifierInSystem(modifier);
                return modifier;
            }
        }

        /// <summary>Inserts given modifier into the system either as active modifier or queued based on the can stack flag</summary>
        private void InsertModifierInSystem(StatModifier modifier)
        {
            if (!modifier.CanStack && activeModifiers.Any(m => m.Name == modifier.Name))
            {
                //insert clone of modifier so stored modifier members of classes won't be modified
                queuedModifiers.Insert(0, modifier);
            }
            else
            {
                activeModifiers.Insert(0, modifier);
            }
        }
    }
}