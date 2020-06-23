using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.StatModification
{
    /// <summary>the main class to be used to show specific stats e.g. hitpoints or energy and modify its state by modifying a current or a max value</summary>
    [Serializable]
    public class StatSystem
    {
        [SerializeField, Min(0)]
        private int max = 1000;

        private List<StatModifier> activeModifiers = new List<StatModifier>();
        private List<StatModifier> queuedModifiers = new List<StatModifier>();

        private Image fillableBar;

        private Text displayText;

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

        /// <summary>Updates systems state based on modifiers stored</summary>
        public void UpdateModifiers()
        {
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

        /// <summary>shows feedback of data on fillablebar and text. Make sure these are not null before using this</summary>
        public void UpdateVisuals()
        {
            fillableBar.fillAmount = Perc;
            displayText.text = $"{Current}/{max}";
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

        /// <summary>Lets a modifier that is in the list of modifiers modify current</summary>
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
                    if (Current > max)
                    {
                        Current = max;
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

        /// <summary>Removes first or all occurences of modifier with given name from the systems queued modifiers list</summary>
        public void RemoveQueuedModifier(string modifierName, bool allOccurences)
        {
            for (int i = queuedModifiers.Count - 1; i >= 0; i--)
            {
                if (queuedModifiers[i].Name == modifierName)
                {
                    queuedModifiers.RemoveAt(i);
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
                OnIncreaseStop?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.Increase) == 0 && !modifier.Increase)
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
            if (activeModifiers.Count(m => m.Increase) == 0 && modifier.Increase)
            {
                OnIncreaseStart?.Invoke();
            }
            else if (activeModifiers.Count(m => !m.Increase) == 0 && !modifier.Increase)
            {
                OnDecreaseStart?.Invoke();
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
            //insert at index zero to make sure when removing a modifier by name, the first instance of this modifier inside the system is being removed
            if (!modifier.CanStack && activeModifiers.Any(m => m.Name == modifier.Name))
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