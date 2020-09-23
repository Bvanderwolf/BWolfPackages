// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>A stat modifier that, over a given amount of time, increase/decreases given amount of value</summary>
    public class TimedStatModifier : StatModifier
    {
        [Min(0)]
        private float time = 0;

        [Min(0)]
        private int value = 0;

        private float timePassed;

        private int valueModified;
        private int currentValue;

        public float Time
        {
            get { return time; }
            set
            {
                if (value >= 0)
                {
                    time = value;
                }
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value >= 0)
                {
                    this.value = value;
                }
            }
        }

        /// <summary>Returns whether all time for this modifier has been passed</summary>
        public override bool Finished
        {
            get { return timePassed >= time; }
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="modifiesCurrentWithMax">Does current value change when max value has changed?</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        public TimedStatModifier(string name, float time, int value, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax, bool canStack)
        {
            this.name = name;
            this.time = time;
            this.value = value;
            this.increase = increase;
            this.modifiesCurrent = modifiesCurrent;
            this.modifiesCurrentWithMax = modifiesCurrentWithMax;
            this.canStack = canStack;
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        public TimedStatModifier(string name, float time, int value, bool increase) : this(name, time, value, increase, true, false, false)
        {
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="canStack">Can this modifier stack with modifiers with the same name?</param>
        public TimedStatModifier(string name, float time, int value, bool increase, bool canStack) : this(name, time, value, increase, true, false, canStack)
        {
        }

        /// <summary>
        /// Creates new instance of a timed stat modifier
        /// </summary>
        /// <param name="name">used for comparing modifiers</param>
        /// <param name="time">Time it takes for this modifier to finish modifying the stat system</param>
        /// <param name="value">The ammount of value it will modify over given amount of time</param>
        /// <param name="increase">Will this modifier increase stat or decrease</param>
        /// <param name="modifiesCurrent">Will this modifier modify current value or max value?</param>
        /// <param name="modifiesCurrentWithMax">Does current value change when max value has changed?</param>
        public TimedStatModifier(string name, float time, int value, bool increase, bool modifiesCurrent, bool modifiesCurrentWithMax = false) : this(name, time, value, increase, modifiesCurrent, modifiesCurrentWithMax, false)
        {
        }

        /// <summary>Modifies system stat based on given time and time passed</summary>
        public override void Modify(StatSystem system)
        {
            if (time == 0f)
            {
                //if time is 0 no calculations have to be done and value can just modify the system once
                if (modifiesCurrent)
                {
                    system.ModifyCurrent(this, increase ? value : -value);
                }
                else
                {
                    system.ModifyMax(this, increase ? value : -value);
                }
                return;
            }

            timePassed += UnityEngine.Time.deltaTime;
            currentValue = (int)(timePassed / time * value);
            if (currentValue != valueModified)
            {
                int difference = Mathf.Abs(currentValue - valueModified);
                valueModified += difference;

                if (currentValue > value)
                {
                    //handle overshot with large numbers and short times
                    int overShot = currentValue - value;
                    difference -= overShot;
                }

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
        }

        /// <summary>Returns a string representation of this TimedStatModifier</summary>
        public override string ToString()
        {
            return $"TimedStatModifier[name: {name}, time: {time}, valuePerSecond: {value}, increase: {increase}, modifiesCurrent: {modifiesCurrent}, modifiesCurrentWithMax: {modifiesCurrentWithMax}, canStack: {canStack}]";
        }
    }
}